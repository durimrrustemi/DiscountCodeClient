namespace DiscountCodeServer.Messages
{
    public class GenerateResponse
    {
        public bool Result { get; set; }

        public static GenerateResponse FromStream(BinaryReader reader)
        {
            return new GenerateResponse
            {
                Result = reader.ReadBoolean()
            };
        }

        public void WriteToStream(BinaryWriter writer)
        {
            writer.Write(Result);
        }
    }
}
