using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEditor;

public static class AuthenticationWrapper 
{
  public static AuthState AuthState { get; private set; } = AuthState.NotAuthenticated;
    public static async Task<AuthState> DoAuth(int Maxtries = 5)
    {
        if (AuthState == AuthState.Authenticated)
        {
            return AuthState;
        }
        if(AuthState == AuthState.Authenticating)
        {
            Debug.LogWarning("Already Authenticating");
            await Authenticating();
            return AuthState;
        }
    await SignInAnonymouslyAsync(Maxtries);
        return AuthState;
    }
    private static async Task<AuthState> Authenticating()
    {
        while (AuthState == AuthState.Authenticating || AuthState == AuthState.NotAuthenticated)
        {
            await Task.Delay(200);
        }
        return AuthState;
    }
    private static async Task SignInAnonymouslyAsync(int Maxtries)
    {
        AuthState = AuthState.Authenticating;
        int tries = 0;
        while (AuthState == AuthState.Authenticating && tries < Maxtries)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                {
                    AuthState = AuthState.Authenticated;
                    break;
                }

            }
            catch(AuthenticationException ex)
            {
                Debug.LogError(ex);
                AuthState = AuthState.Error;
            }
            catch (RequestFailedException failex)
            {
                Debug.LogError(failex);
                AuthState = AuthState.Error;
            }

            tries++;
            await Task.Delay(1000);
        }
        if(AuthState != AuthState.Authenticated)
        {
            Debug.LogWarning($"Player was not signin succesfully after {tries } tries");
            AuthState = AuthState.TimeOut;
        }
    }
}
public enum AuthState
{
    NotAuthenticated,
    Authenticating,
    Authenticated,
    Error,
    TimeOut
}