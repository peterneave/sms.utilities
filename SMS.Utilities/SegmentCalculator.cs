namespace SMS.Utilities
{
    public class SegmentCalculator
    {
        //https://www.twilio.com/docs/glossary/what-sms-character-limit
        //https://en.wikipedia.org/wiki/GSM_03.38
        //https://messente.com/documentation/tools/sms-length-calculator

        private const string GSM0338 = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ@£$¥èéùìòÇ\rØø\nÅåΔ_ΦΓΛΩΠΨΣΘΞ\u001bÆæßÉ !\"#¤%&'()*+,-./:;<=>?¡ÄÖÑÜ§¿äöñüà";

        private const string GSM0338ext = "|^€{}[]~";
        private const int GSMSingleSegmentLimit = 160;
        private const int UnicodeSingleSegmentLimit = 70;
        private const int GSMMultiSegmentLimit = 153;
        private const int UnicodeMultiSegmentLimit = 67;

        public enum Charset
        {
            Undetermined,
            GSM,
            Unicode
        }

        public (int segments, Charset charset) GetSegmentsCount(string message)
        {
            var messageChars = message.ToCharArray();

            var count = 0;
            var charset = Charset.Undetermined;

            foreach (var c in messageChars)
            {
                charset = Charset.Undetermined;
                foreach (var gsmc in GSM0338.ToCharArray())
                {
                    if (c == gsmc)
                    {
                        charset = Charset.GSM;
                        break;
                    }
                }

                if (charset != Charset.GSM)
                {
                    foreach (var gsmce in GSM0338ext.ToCharArray())
                    {
                        if (c == gsmce)
                        {
                            charset = Charset.GSM;
                            //add for escape character for extended GSM
                            count++;
                        }
                    }
                }

                if (charset != Charset.GSM)
                {
                    charset = Charset.Unicode;
                    count = messageChars.Length;
                    break;
                }

                count++;
            }

            var isGSM = charset == Charset.GSM;
            var singleSegmentLimit = isGSM ? GSMSingleSegmentLimit : UnicodeSingleSegmentLimit;
            var multiSegmentLimit = isGSM ? GSMMultiSegmentLimit : UnicodeMultiSegmentLimit;

            return ((count <= singleSegmentLimit) ? 1 : GetSegments(count, multiSegmentLimit), charset);
        }

        private int GetSegments(int encodedLength, int segmentLength)
        {
            if (encodedLength < segmentLength) return 1;

            var r = encodedLength % segmentLength;
            return ((encodedLength - r) / segmentLength) + 1;
        }
    }
}