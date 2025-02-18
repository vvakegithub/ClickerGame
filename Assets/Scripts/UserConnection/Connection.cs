using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Connection : MonoBehaviour
{
    public string ReferralLink => _referralLink;
    public string ReferralId => _referralId;

    [SerializeField] private string _serverURI = "https://example.com/highscore/";

    private string _playerId = "";
    private string _secretKey = "";
    private string _referralLink = "";
    private string _referralId = "";

    private bool _dontSend = false;

    private void Awake()
    {
        
    }

    private void Start()
    {
#if UNITY_EDITOR
        _dontSend = true;
#elif UNITY_WEBGL
        if (URLParameters.GetSearchParameters().ContainsKey("start"))
        {
            _referralId = URLParameters.GetSearchParameters()["start"];
        }
        else if (URLParameters.GetSearchParameters().ContainsKey("id"))
        {
            _playerId = URLParameters.GetSearchParameters()["id"];
        }

        _secretKey = GenerateSecretKey(_playerId);
        _referralLink = GenerateReferralLink(_playerId);
#endif
    }

    public int GetScore()
    {
        if (_dontSend || _playerId == "")
        {
            return 0;
        }

        string uri = _serverURI + "?id=" + _playerId;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            UnityWebRequestAsyncOperation asyncOperation = webRequest.SendWebRequest();
            while (!asyncOperation.isDone)
            {
                
            }

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError("Error getting score: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error getting score: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("Error getting score: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    string encryptedScore = webRequest.downloadHandler.text;
                    int score = Deobfuscate(long.Parse(_playerId), encryptedScore);
                    Debug.Log("Success getting score: " + score);
                    return score;
            }
        }

        return 0;
    }

    public void SendScore(int score)
    {
        if (_dontSend || _playerId == "")
        {
            return;
        }

        long obfuscatedScore = Obfuscate(long.Parse(_playerId), score);

        string uri = _serverURI + obfuscatedScore.ToString() + "?id=" + _playerId;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            UnityWebRequestAsyncOperation asyncOperation = webRequest.SendWebRequest();
            while (!asyncOperation.isDone)
            {
                
            }

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError("Error sending score: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error sending score: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("Error sending score: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Success sending score");
                    break;
            }
        }
    }

    private string GenerateSecretKey(string playerId)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(playerId);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

    private byte[] GenerateUniqueKey(long playerId)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(_secretKey);
            aes.IV = BitConverter.GetBytes(playerId);

            using (var encryptor = aes.CreateEncryptor())
            {
                return encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(_secretKey), 0, _secretKey.Length);
            }
        }
    }

    private string GenerateReferralLink(string playerId)
    {
        string referralLink = "https://t.me/<bot_username>?start=" + _playerId;

        return referralLink;
    }

    private long Obfuscate(long playerId, int score)
    {
        byte[] uniqueKey = GenerateUniqueKey(playerId);

        using (var aes = Aes.Create())
        {
            aes.Key = uniqueKey;
            aes.IV = new byte[aes.BlockSize / 8];

            using (var encryptor = aes.CreateEncryptor())
            {
                byte[] scoreBytes = BitConverter.GetBytes(score);
                byte[] encryptedScore = encryptor.TransformFinalBlock(scoreBytes, 0, scoreBytes.Length);
                return BitConverter.ToInt64(encryptedScore, 0);
            }
        }
    }

    private int Deobfuscate(long playerId, string encryptedScore)
    {
        byte[] uniqueKey = GenerateUniqueKey(playerId);

        using (var aes = Aes.Create())
        {
            aes.Key = uniqueKey;
            aes.IV = new byte[aes.BlockSize / 8];

            using (var decryptor = aes.CreateDecryptor())
            {
                byte[] encryptedScoreBytes = Convert.FromBase64String(encryptedScore);
                byte[] decryptedScoreBytes = decryptor.TransformFinalBlock(encryptedScoreBytes, 0, encryptedScoreBytes.Length);
                return BitConverter.ToInt32(decryptedScoreBytes, 0);
            }
        }
    }
}
