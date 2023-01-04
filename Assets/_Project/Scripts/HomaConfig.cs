using HomaGames.HomaBelly;

public class HomaConfig : Singleton<HomaConfig>
{
	private static bool _init = false;

	protected override void OnAwakeEvent()
	{
		if (!HomaBelly.Instance.IsInitialized)
		{
			// Listen event for initialization
			Events.onInitialized += OnInitialized;
		}
		else
		{
			// Homa Belly already initialized
		}
	}

	public override void OnDisable()
	{
		base.OnDisable();
		Events.onInitialized -= OnInitialized;
	}

	private void OnInitialized()
	{
		// Homa Belly initialized, call any Homa Belly method 
		_init = true;
	}


	/// <summary>
	/// Invoke this method everytime your Main Menu screen is loaded
	/// </summary>
	public static void MainMenuLoaded()
	{
		if (_init)
		{
			DefaultAnalytics.MainMenuLoaded();
		}
	}


	/// <summary>
	/// Invoke this method everytime the user starts the gameplay at any level
	/// </summary>
	/// 
	public static void GameplayStarted()
	{
		if (_init)
		{
			DefaultAnalytics.GameplayStarted();
		}
	}

	/// <summary>
	/// Invoke this every time player starts the level. Levels should start at 1
	/// </summary>
	/// <param name="levelId">The level id started</param>
	public static void LevelStarted(string levelId)
	{
		if (_init)
		{
			DefaultAnalytics.LevelStarted(levelId);
		}
	}

	/// <summary>
	/// Invoke this every time player fails the level
	/// </summary>
	public static void LevelFailed()
	{
		if (_init)
		{
			DefaultAnalytics.LevelFailed();
		}
	}

	/// <summary>
	/// Invoke this every time player successfully completes current level, the one tracked
	/// with DefaultAnalytics.LevelStarted() method
	/// </summary>
	public static void LevelCompleted()
	{
		if (_init)
		{
			DefaultAnalytics.LevelCompleted();
		}
	}

	/// <summary>
	/// Invoke this method everytime a tutorial step is started. Invoking
	/// it twice for the same step is harmless, as only the very first
	/// one will be taken into account.
	/// </summary>
	/// <param name="step">The tutorial step</param>
	public static void TutorialStepStarted(string tutorialStep)
	{
		if (_init)
		{
			DefaultAnalytics.TutorialStepStarted(tutorialStep);
		}
	}

	/// <summary>
	/// Invoke this every time the player does not execute the asked 
	/// behavior in the current tutorial step
	/// </summary>
	public static void TutorialStepFailed()
	{
		if (_init)
		{
			DefaultAnalytics.TutorialStepFailed();
		}
	}

	/// <summary>
	/// Invoke this method everytime a tutorial step is completed. Invoking
	/// it twice for the same step is harmless, as only the very first
	/// one will be taken into account.
	/// </summary>
	public static void TutorialStepCompleted()
	{
		if (_init)
		{
			DefaultAnalytics.TutorialStepCompleted();
		}
	}

	/// <summary>
	/// Invoke this method whenever a rewarded offer is suggested to the player. There is no need to track
	/// any other Ad related event as Homa Belly will automatically take care of the rest.
	/// </summary>
	/// <param name="rewardedAdName">Please follow the nomenclature in the relevant document</param>
	public static void SuggestedRewardedAd(string rewardedAdName = "")
	{
		if (_init)
		{
			DefaultAnalytics.SuggestedRewardedAd(rewardedAdName);
		}
	}
}
