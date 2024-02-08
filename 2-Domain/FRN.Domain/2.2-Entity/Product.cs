using System.ComponentModel.DataAnnotations;

namespace FRN.Domain._2._2_Entity
{
    public class Product
    {
        public int cod_produto { get; set; }
        public string descricao { get; set; }
        public bool situacao_produto { get; set; }
        public DateTime dt_fabricacao { get; set; }
        public DateTime dt_validade { get; set; }
        public int cod_fornecedor { get; set; }
        public string descricao_fornecedor { get; set; }

        public class DateNotLessThanAttribute : ValidationAttribute
        {
            private readonly string _comparisonProperty;

            public DateNotLessThanAttribute(string comparisonProperty)
            {
                _comparisonProperty = comparisonProperty;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var propertyInfo = validationContext.ObjectType.GetProperty(_comparisonProperty);

                if (propertyInfo == null)
                {
                    return new ValidationResult($"Propriedade {_comparisonProperty} não encontrada.");
                }

                var comparisonValue = (DateTime)propertyInfo.GetValue(validationContext.ObjectInstance);

                if ((DateTime)value < comparisonValue)
                {
                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }
        }
    }
}
