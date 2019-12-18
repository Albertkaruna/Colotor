using System;
using System.Collections.Generic;
using UnityEngine;

// Placing the Purchaser class in the CompleteProject namespace allows it to interact with Sc oreManager,
// one of the existing Survival Shooter scripts.

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class IAPController : MonoBehaviour
{


//	public static IAPController instance;


//	private static IStoreController m_StoreController;
//	// The Unity Purchasing system.
//	private static IExtensionProvider m_StoreExtensionProvider;

//	public static string Hints_5 = "hints5";
//	public static string Hints_10 = "hints10";
//	public static string Hints_15 = "hints15";

//	// Apple App Store-specific product identifier for the subscription product.
//	private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";

//	// Google Play Store-specific product identifier subscription product.
//	private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

//	void Start ()
//	{
//		// If we haven't set up the Unity Purchasing reference
//		if (m_StoreController == null) {
//			// Begin to configure our connection to Purchasing
//			InitializePurchasing ();
//		}

//		if (instance != null) {
//			Destroy (gameObject);
//		} else {
//			instance = this;
//			DontDestroyOnLoad (gameObject);
//		}
//	}

//	public void InitializePurchasing ()
//	{
//		// If we have already connected to Purchasing ...
//		if (IsInitialized ()) {
//			// ... we are done here.
//			return;
//		}

//		// Create a builder, first passing in a suite of Unity provided stores.
//		var builder = ConfigurationBuilder.Instance (StandardPurchasingModule.Instance ());


//		builder.AddProduct (Hints_5, ProductType.Consumable);
//		builder.AddProduct (Hints_10, ProductType.Consumable);
//		builder.AddProduct (Hints_15, ProductType.Consumable);


//		UnityPurchasing.Initialize (this, builder);
//	}


//	private bool IsInitialized ()
//	{
//		// Only say we are initialized if both the Purchasing references are set.
//		return m_StoreController != null && m_StoreExtensionProvider != null;
//	}


//	public void BuyHints5 ()
//	{
//		// Buy the consumable product using its general identifier. Expect a response either 
//		// through ProcessPurchase or OnPurchaseFailed asynchronously.
//		BuyProductID (Hints_5);
//	}

//	public void BuyHints10 ()
//	{
//		// Buy the consumable product using its general identifier. Expect a response either 
//		// through ProcessPurchase or OnPurchaseFailed asynchronously.
//		BuyProductID (Hints_10);
//	}

//	public void BuyHints15 ()
//	{
//		// Buy the consumable product using its general identifier. Expect a response either 
//		// through ProcessPurchase or OnPurchaseFailed asynchronously.
//		BuyProductID (Hints_15);
//	}


//	void BuyProductID (string productId)
//	{
//		// If Purchasing has been initialized ...
//		if (IsInitialized ()) {
//			// ... look up the Product reference with the general product identifier and the Purchasing 
//			// system's products collection.
//			Product product = m_StoreController.products.WithID (productId);

//			// If the look up found a product for this device's store and that product is ready to be sold ... 
//			if (product != null && product.availableToPurchase) {
//				Debug.Log (string.Format ("Purchasing product asychronously: '{0}'", product.definition.id));
//				// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
//				// asynchronously.
//				m_StoreController.InitiatePurchase (product);
//			}
//				// Otherwise ...
//				else {
//				// ... report the product look-up failure situation  
//				Debug.Log ("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
//			}
//		}
//			// Otherwise ...
//			else {
//			// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
//			// retrying initiailization.
//			Debug.Log ("BuyProductID FAIL. Not initialized.");
//		}
//	}


//	// Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google.
//	// Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
//	public void RestorePurchases ()
//	{
//		// If Purchasing has not yet been set up ...
//		if (!IsInitialized ()) {
//			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
//			Debug.Log ("RestorePurchases FAIL. Not initialized.");
//			return;
//		}

//		// If we are running on an Apple device ... 
//		if (Application.platform == RuntimePlatform.IPhonePlayer ||
//		    Application.platform == RuntimePlatform.OSXPlayer) {
//			// ... begin restoring purchases
//			Debug.Log ("RestorePurchases started ...");

//			// Fetch the Apple store-specific subsystem.
//			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions> ();
//			// Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
//			// the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
//			apple.RestoreTransactions ((result) => {
//				// The first phase of restoration. If no more responses are received on ProcessPurchase then 
//				// no purchases are available to be restored.
//				Debug.Log ("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
//			});
//		}
//			// Otherwise ...
//			else {
//			// We are not running on an Apple device. No work is necessary to restore purchases.
//			Debug.Log ("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
//		}
//	}


//	public void OnInitialized (IStoreController controller, IExtensionProvider extensions)
//	{
//		// Purchasing has succeeded initializing. Collect our Purchasing references.
//		Debug.Log ("OnInitialized: PASS");

//		// Overall Purchasing system, configured with products for this application.
//		m_StoreController = controller;
//		// Store specific subsystem, for accessing device-specific store features.
//		m_StoreExtensionProvider = extensions;
//	}


//	public void OnInitializeFailed (InitializationFailureReason error)
//	{
//		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
//		Debug.Log ("OnInitializeFailed InitializationFailureReason:" + error);
//	}


//	public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs args)
//	{
//		// A consumable product has been purchased by this user.
//		if (String.Equals (args.purchasedProduct.definition.id, Hints_5, StringComparison.Ordinal)) {
//			GameController.instance.Hints += 5;
//			GameController.instance.SetHintsTxt (GameController.instance.Hints);
//			GameController.instance.SetRemainingHintsTxt (GameController.instance.Hints);
//			PlayerPrefController.instance.SetHintsKey (GameController.instance.Hints);

//			Debug.Log ("You've just bought 5 hints");
//		} else if (String.Equals (args.purchasedProduct.definition.id, Hints_10, StringComparison.Ordinal)) {
//			GameController.instance.Hints += 10;
//			GameController.instance.SetHintsTxt (GameController.instance.Hints);
//			GameController.instance.SetRemainingHintsTxt (GameController.instance.Hints);
//			PlayerPrefController.instance.SetHintsKey (GameController.instance.Hints);

//			Debug.Log ("You've just bought 10 hints");
//		} else if (String.Equals (args.purchasedProduct.definition.id, Hints_15, StringComparison.Ordinal)) {
//			GameController.instance.Hints += 15;
//			GameController.instance.SetHintsTxt (GameController.instance.Hints);
//			GameController.instance.SetRemainingHintsTxt (GameController.instance.Hints);
//			PlayerPrefController.instance.SetHintsKey (GameController.instance.Hints);

//			Debug.Log ("You've just bought 15 hints");
//		} else {
////			Some unknows product has been purchased by this user.
//			Debug.Log (string.Format ("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
//		}

//		// Return a flag indicating whether this product has completely been received, or if the application needs 
//		// to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
//		// saving purchased products to the cloud, and when that save is delayed. 
//		return PurchaseProcessingResult.Complete;
//	}


//	public void OnPurchaseFailed (Product product, PurchaseFailureReason failureReason)
//	{
//		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
//		// this reason with the user to guide their troubleshooting actions.
//		Debug.Log (string.Format ("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
//	}
}