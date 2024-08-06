namespace Library.Model.Enums;

public enum BookCopyStatus
{
    Normal, // defaults to this
    Damaged,
    Lost,
    LostAndReturnedAnotherCopy // the copy is lost, but customer is not in fault anymore
}
