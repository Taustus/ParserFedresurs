using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserFedresource
{
    public class Info
    {
        public string URL { get; set; } 
        public string MessageNumber { get; set; }
        public string Contract { get; set; }
        public string Lessor { get; set; }
        public string LessorINN { get; set; }
        public string ContractNumber { get; set; }
        public string RentalPeriod { get; set; }
        public string Pledger { get; set; }
        public string PledgerINN { get; set; }
        public string Identifier { get; set; }
        public string Classification { get; set; }
        public string Description { get; set; }
        public string LinkedMessages { get; set; }
        public string File { get; set; }
        public override string ToString()
        {
            return $"Message number: {MessageNumber}\nContract: {Contract}\nLessor: {Lessor}\n" +
                $"LessorINN: {LessorINN}\nContractNumber: {ContractNumber}\nRentalPeriod: {RentalPeriod}\n" +
                $"Pledger: {Pledger}\nPledgerINN: {PledgerINN}\nIdentifier: {Identifier}\nClassification: {Classification}\n" +
                $"Description: {Description}\nLinkedMessages: {LinkedMessages}\nFile: {File}";
        }
    }
}
