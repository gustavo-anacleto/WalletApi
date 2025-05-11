namespace WalletApi.Presentation.DTO;

public class KeycloakUserDto
{
    public string Email { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
    public bool EmailVerified  { get; set; } = true;
    public AttributesDto Attributes  { get; set; } = null! ;
    public List<CredentialDto> Credentials { get; set; } = [];
}