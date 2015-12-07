using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 *  Generate the Challenge
	Call method CreateRandomChallenge() on YubiKeyEncryptor.cs
Authenticate the Yubikey:
	with the callenge generated before and the response from the yubikey
	Call the method AuthenticateYubiKey() on SecurityLock.cs:
		LoadAESKey() call this store procedure sec_sp_Auth_Transaction_Verify_GetAgentValue
		with the result call the mehtod Decrypt() and then GetUID() on YubiKeyEncryptor.cs
		if the result as uid != null then call LoadYubiKeyUID () in YubiKeyAuthentication.cs that call the SP sec_sp_Auth_Transaction_Verify_ObjectValue
		If the result of tha SP the output var @fOBjectID is > 0 then the validation was ok
 * */
namespace Yubikey
{
    class Program
    {
       
        static void Main(string[] args)
        {
             string user = "admin";

             string agentId = "165914";

             string challenge = (new YubikeyUtilities().GetChallenge());

             Console.WriteLine("\nPlease press yubikey button: ");
             string yubikeyValue = Console.ReadLine(); //"iirgeikluvnkdeuihduihfjlkcbnchck";
             yubikeyValue = yubikeyValue.Trim();

             var response = (new YubikeyUtilities().Authenticate(user, agentId, challenge, yubikeyValue));

             Console.WriteLine("\n\nIS Unlocked: "+response.IsUnlocked.ToString());
             Console.WriteLine("\n\nObjectId: " + response.ObjectId.ToString());
             Console.WriteLine("\n\nValueId: " + response.ValueId.ToString());

             Console.ReadLine();
        }
    }
}
