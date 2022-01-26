using System.Collections.Generic;

namespace LibRtDb.DTO.Languages
{
    public class LanguageResource
    {
        public long Id { get; set; }
        public string Language { get; set; }
        public List<LangRes> Resources { get; set; }
    }
}
