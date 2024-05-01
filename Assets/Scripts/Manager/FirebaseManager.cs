using Firebase;
using Firebase.Auth;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private static FirebaseManager instance;
    public static FirebaseManager Instance { get { return instance; } }

    private static FirebaseApp app;
    public static FirebaseApp App { get { return app; } }

    private static FirebaseAuth auth;
    public static FirebaseAuth Auth { get { return auth; } }


    private static bool isValid;
    public static bool IsValid { get { return isValid; } }

    private void Awake()
    {
        CreateInstance();
        CheckDependency();
    }

    private void CreateInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async void CheckDependency()
    {
        DependencyStatus dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            // Create and hold a reference to your FirebaseApp,
            // where app is a Firebase.FirebaseApp property of your application class.
            app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;

            // Set a flag here to indicate whether Firebase is ready to use by your app.
            Debug.Log("Firebase Check and FixDependencies success");
            isValid = true;
        }
        else
        {
            // Firebase Unity SDK is not safe to use here.
            Debug.LogError("Firebase Check and FixDependencies fail");
            isValid = false;

            app = null;
            auth = null;
        }
    }
}
