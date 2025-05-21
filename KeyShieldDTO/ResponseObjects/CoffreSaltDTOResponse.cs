namespace KeyShieldDTO.ResponseObjects;

public class CoffreSaltDTOResponse(byte[] salt)
{
    public byte[] Salt { get; set; } = salt;
}