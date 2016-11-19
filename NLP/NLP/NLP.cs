using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using opennlp.tools.sentdetect;
using opennlp.tools.tokenize;
using opennlp.tools.postag;
using opennlp.tools.chunker;
using opennlp.tools.parser;
using opennlp.tools.util;
using System.IO;
using opennlp.tools.cmdline.parser;
using LemmaSharp;

namespace NLP_Project
{

    class NLP
    {
        private SentenceDetectorME sentenceDetector;
        private TokenizerME tokenizer;
        private POSTaggerME tagger;
        private ChunkerME chunker;
        private Parser parser;
        ILemmatizer lmtz;
        private HashSet<string> stopwords = new HashSet<string>();

        bool alreadyLoadSentenceDetector = false;
        bool alreadyLoadTokenizer = false;
        bool alreadyLoadTagger = false;
        bool alreadyLoadChunker = false;
        bool alreadyLoadStopwords = false;
        bool alreadyLoadParser = false;
        bool alreadyLoadLemmatizer = false;

        private void LoadSentenceDetector()
        {
            if (!alreadyLoadSentenceDetector)
            {
                java.io.FileInputStream modelInpStream = new java.io.FileInputStream("Resources\\en-sent.bin");
                SentenceModel sentenceModel = new SentenceModel(modelInpStream);
                sentenceDetector = new SentenceDetectorME(sentenceModel);

                alreadyLoadSentenceDetector = true;
            }
        }

        private void LoadTokenizer()
        {
            if (!alreadyLoadTagger)
            {
                java.io.FileInputStream modelInpStream = new java.io.FileInputStream("Resources\\en-token.bin");
                TokenizerModel tokenizerModel = new TokenizerModel(modelInpStream);
                tokenizer = new TokenizerME(tokenizerModel);

                alreadyLoadTagger = true;
            }
        }

        private void LoadTagger()
        {
            if (!alreadyLoadTokenizer)
            {
                java.io.FileInputStream modelInpStream = new java.io.FileInputStream("Resources\\en-pos-maxent.bin");
                POSModel posModel = new POSModel(modelInpStream);
                tagger = new POSTaggerME(posModel);

                alreadyLoadTokenizer = true;
            }
        }

        private void LoadChunker()
        {
            if (!alreadyLoadChunker)
            {
                java.io.FileInputStream modelInpStream = new java.io.FileInputStream("Resources\\en-chunker.bin");
                ChunkerModel chunkerModel = new ChunkerModel(modelInpStream);
                chunker = new ChunkerME(chunkerModel);

                alreadyLoadChunker = true;
            }
        }

        private void LoadParser()
        {
            if (!alreadyLoadParser)
            {
                java.io.FileInputStream modelInpStream = new java.io.FileInputStream("Resources\\en-parser-chunking.bin");
                ParserModel parserModel = new ParserModel(modelInpStream);
                parser = ParserFactory.create(parserModel);

                alreadyLoadParser = true;
            }
        }

        private void LoadLemmatizer()
        {
            if (!alreadyLoadLemmatizer)
            {
                lmtz = new LemmatizerPrebuiltCompact(LemmaSharp.LanguagePrebuilt.English);

                alreadyLoadLemmatizer = true;
            }
        }

        private void LoadStopwords()
        {
            if (!alreadyLoadStopwords)
            {
                using (StreamReader sr = new StreamReader("Resources\\english.stop.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        stopwords.Add(Stem(line));
                        stopwords.Add(line);
                    }
                }

                alreadyLoadStopwords = true;
            }
        }

        CKIPSS ckip = new CKIPSS("iisr", "123456");
        /// <summary>
        /// 使用CKIP來斷詞、詞性標註。
        /// </summary>
        /// <param name="str">中文文章，太長請分多次送。</param>
        /// <returns>string[0] -> 第一句, string[1] -> 第二句 ...</returns>

        public string[] CWS(string str)
        {
            CKIPSS ckip = new CKIPSS("iisr", "123456");

            bool isSuccess = false;
            string errorMsg = "";

            List<string>  result = ckip.Send(str, out isSuccess, out errorMsg);

            if (isSuccess)
                return result.ToArray();
            else
                throw new Exception(errorMsg);
        }

        /// <summary>
        /// Return bigram of given array.
        /// </summary>
        /// <param name="terms"></param>
        /// <returns>Return bigram of given array.</returns>
        public Tuple<string, string>[] Bigrams(string[] terms)
        {
            List<Tuple<string, string>> result = new List<Tuple<string, string>>();
            for (int i = 1; i < terms.Length - 1; i++)
                result.Add(new Tuple<string, string>(terms[i - 1], terms[i]));

            return result.ToArray();
        }

        //
        /// <summary>
        /// 使用openNLP英文斷句。model: en-sent.bin。
        /// </summary>
        /// <param name="str">需要斷句的英文文章。</param>
        /// <returns>string[0] -> 第一句, string[1] -> 第二句 ...</returns>
        public string[] SentDetect(string str)
        {
            LoadSentenceDetector();
            return sentenceDetector.sentDetect(str);
        }

        /// <summary>
        /// 使用openNLP英文斷詞。model: en-token.bin。
        /// </summary>
        /// <param name="sentence">需要斷詞的英文句子。</param>
        /// <returns>斷完的每個Token</returns>
        public string[] Tokenize(string sentence)
        {
            LoadTokenizer();
            Span[] tokenSpans = tokenizer.tokenizePos(sentence);
            List<String> list = new List<String>();

            foreach (Span span in tokenSpans)
                list.Add(sentence.Substring(span.getStart(), span.getEnd() - span.getStart()));

            return list.ToArray();
        }

        /// <summary>
        /// 使用openNLP做英文詞性標註。model: en-pos-maxent.bin。
        /// </summary>
        /// <param name="tokens">每個需要標註的tokens。</param>
        /// <returns>POS array</returns>
        public string[] POS(string[] tokens)
        {
            LoadTagger();
            return tagger.tag(tokens);
        }

        /// <summary>
        /// 使用openNLP做英文Chunker。model: en-chunker.bin。
        /// </summary>
        /// <param name="tokens">每個需要標註的tokens。</param>
        /// <returns></returns>
        public string[] Chunk(string[] tokens)
        {
            LoadTagger();
            string[] pos = tagger.tag(tokens);
            return Chunk(tokens, pos);
        }

        /// <summary>
        /// 使用openNLP做英文Chunker。model: en-chunker.bin。
        /// </summary>
        /// <param name="tokens">每個需要標註的tokens。</param>
        /// <param name="pos">tokens的POS結果</param>
        /// <returns></returns>
        public string[] Chunk(string[] tokens, string[] pos)
        {
            LoadChunker();
            return chunker.chunk(tokens, pos);
        }

        /// <summary>
        /// 使用openNLP做英文Parsing。model: en-parser-chunking.bin。
        /// </summary>
        /// <param name="sentence">英文句子。</param>
        /// <returns>樹狀資料結構。</returns>
        public Parse Parser(string sentence)
        {
            if (sentence.Length <= 0)
                return null;

            LoadParser();
            string result = String.Empty;
            Parse[] topParses = ParserTool.parseLine(sentence, parser, 1);

            return topParses[0];
        }

        /// <summary>
        /// 使用LemmaGen做lemmatization。
        /// </summary>
        /// <param name="word">需要做lemmatization的字。</param>
        /// <returns>lemmatization後的結果。</returns>
        public string Lemmatization(string word)
        {
            LoadLemmatizer();
            string wordLower = word.ToLower();
            return lmtz.Lemmatize(wordLower);
        }

        /// <summary>
        /// 使用LemmaGen做lemmatization。
        /// </summary>
        /// <param name="words">多個需要lemmatization的字。</param>
        /// <returns>lemmatization後的結果。</returns>
        public string[] Lemmatization(string[] words)
        {
            List<string> result = new List<string>();
            foreach (string s in words)
                result.Add(Lemmatization(s));

            return result.ToArray();
        }

        /// <summary>
        /// 使用Porter stemmer做stemming。
        /// </summary>
        /// <param name="token">需要做stemming的字。</param>
        /// <returns>stemming後的結果。</returns>
        public string Stem(string token)
        {
            return Stemmer.go(token);
        }

        /// <summary>
        /// 使用Porter stemmer做stemming。
        /// </summary>
        /// <param name="token">多個需要做stemming的字。</param>
        /// <returns>stemming後的結果。</returns>
        public string[] Stem(string[] words)
        {
            List<string> result = new List<string>();
            foreach (string s in words)
                result.Add(Stem(s));

            return result.ToArray();
        }

        /// <summary>
        /// 過濾掉array內的停用詞。
        /// 使用Smart stop list: http://jmlr.org/papers/volume5/lewis04a/a11-smart-stop-list/english.stop。
        /// </summary>
        /// <param name="token"></param>
        /// <returns>過濾後的結果。</returns>
        public string[] FilterOutStopWords(string[] words)
        {
            List<string> result = new List<string>();

            foreach (string word in words)
                if (!IsStopWord(word))
                    result.Add(word);

            return result.ToArray();
        }

        /// <summary>
        /// 是否為停用詞。
        /// 使用Smart stop list: http://jmlr.org/papers/volume5/lewis04a/a11-smart-stop-list/english.stop。
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool IsStopWord(string word)
        {
            LoadStopwords();
            string lowerCase = word.ToLower();

            //過濾掉完全沒有包含英文的token
            bool ok = false;
            foreach (char c in lowerCase)
            {
                if (c >= 'a' && c <= 'z')
                {
                    ok = true;
                    break;
                }
            }

            if (ok)
                return stopwords.Contains(lowerCase);
            else
                return true;
        }
    }
}
