using System.Text;

namespace Ballastlane.Authentications;

public class JwtTokenConfiguration
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }

    private byte[] byteKey;
    public byte[] GetKey()
    {
        byteKey ??= Encoding.UTF8.GetBytes(Key);

        return byteKey;
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Key))
            throw new ArgumentException(nameof(Key));
        if (string.IsNullOrWhiteSpace(Issuer))
            throw new ArgumentException(nameof(Issuer));
        if (string.IsNullOrWhiteSpace(Audience))
            throw new ArgumentException(nameof(Audience));

        _ = GetKey();
    }
}
