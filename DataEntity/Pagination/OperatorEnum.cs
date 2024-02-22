namespace DataEntity.Pagination
{
    public enum OperatorEnum
    {
        None,

        Equal,
        NotEqual,

        Between,
        NotBetween,

        StartWith,
        EndWith,

        NotStartWith,
        NotEndWith,

        InSet,
        NotInSet,

        Contain,
        NotContain
    }
}
