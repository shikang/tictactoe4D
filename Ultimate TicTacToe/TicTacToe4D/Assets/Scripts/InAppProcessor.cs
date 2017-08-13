using UnityEngine;
using UnityEngine.Analytics;
using System;
using System.Collections.Generic;

public class InAppProcessor : Singleton<InAppProcessor>
{
	public class ProductParam
	{
		public InAppProductList.ProductType m_ProductType;
		public int m_nProductParam;

		public ProductParam( InAppProductList.ProductType productType, int productParam )
		{
			m_ProductType = productType;
			m_nProductParam = productParam;
		}
	}

	private Dictionary<string, ProductParam> m_ProductParamMap;

	protected InAppProcessor()
	{
		m_ProductParamMap = new Dictionary<string, ProductParam>();
	}

	public void AddProductParam( string productIdentifier, InAppProductList.ProductType productType, int productParam)
	{
		m_ProductParamMap.Add( productIdentifier, new ProductParam( productType, productParam ) );
	}

	public void ProcessPurchase( string productIdentifier )
	{
		if( m_ProductParamMap.ContainsKey( productIdentifier ) )
		{
			ProductParam productParam = m_ProductParamMap[productIdentifier];

			switch ( productParam.m_ProductType )
			{
				case InAppProductList.ProductType.COIN:
					GameData.current.coin += productParam.m_nProductParam;
					Debug.Log( string.Format( "InAppProcessor::ProcessPurchase: PASS. Product: '{0}'", productIdentifier ) );

					// @todo Some feedback
					break;
				case InAppProductList.ProductType.AVATAR:
					Debug.Log( string.Format( "InAppProcessor::ProcessPurchase: PASS. Product: '{0}'", productIdentifier ) );

					GameObject go = GameObject.FindGameObjectWithTag( "Gacha" );
					GachaScript gacha = go.GetComponent<GachaScript>();
					gacha.ProcessBuyIcon( productParam.m_nProductParam );

					Analytics.CustomEvent("AvatarPurchased", new Dictionary<string, object>
					{
						{"AvatarPurchased", ((Defines.ICONS)productParam.m_nProductParam).ToString() }
					});
					break;

				case InAppProductList.ProductType.ADS:
					Defines.AdsInAppPurchase adsProductParam = (Defines.AdsInAppPurchase)productParam.m_nProductParam;

					switch ( adsProductParam )
					{
						case Defines.AdsInAppPurchase.DISABLE:
							Debug.Log( string.Format( "InAppProcessor::ProcessPurchase: PASS. Product: '{0}'", productIdentifier ) );
							Adverts.Instance.RemoveAds();

							// Disable disable ads button
							MainMenuScript mainMenu = Camera.main.GetComponent<MainMenuScript>();
							mainMenu.EnableDisableAdsButton(true);
							mainMenu.DisableDisableAdsButton();

							Analytics.CustomEvent("AdsPurchased", new Dictionary<string, object>
							{
								{"AdsPurchased", 1}
							});
							break;
						default:
							Debug.Log( string.Format( "InAppProcessor::ProcessPurchase: FAIL. Unrecognized Ads Product Params: '{0}'", productIdentifier ) );
							break;
					}
					
					break;
				default:
					Debug.Log( string.Format( "InAppProcessor::ProcessPurchase: FAIL. Invalid product type: '{0}'", productParam.m_ProductType.ToString() ) );
					return;
			}

			SaveLoad.Save();
		}
		else
		{
			Debug.Log( string.Format( "InAppProcessor::ProcessPurchase: FAIL. Unrecognized product: '{0}'", productIdentifier ) );
		}
	}

	public void ProcessPurchaseFailed( string productIdentifier )
	{
		if( m_ProductParamMap.ContainsKey( productIdentifier ) )
		{
			ProductParam productParam = m_ProductParamMap[productIdentifier];

			switch ( productParam.m_ProductType )
			{
				case InAppProductList.ProductType.COIN:
					Debug.Log( string.Format( "InAppProcessor::ProcessPurchase: FAIL. Product: '{0}'", productIdentifier ) );

					break;
				case InAppProductList.ProductType.AVATAR:
					Debug.Log( string.Format( "InAppProcessor::ProcessPurchase: FAIL. Product: '{0}'", productIdentifier ) );

					GameObject go = GameObject.FindGameObjectWithTag( "Gacha" );
					GachaScript gacha = go.GetComponent<GachaScript>();
					gacha.EnableBuyUI( true );
					break;
				case InAppProductList.ProductType.ADS:
					Debug.Log( string.Format( "InAppProcessor::ProcessPurchase: FAIL. Product: '{0}'", productIdentifier ) );

					MainMenuScript mainMenu = Camera.main.GetComponent<MainMenuScript>();
					mainMenu.EnableDisableAdsButton(true);
					break;
				default:
					Debug.Log( string.Format( "InAppProcessor::ProcessPurchase: FAIL. Invalid product type: '{0}'", productParam.m_ProductType.ToString() ) );
					return;
			}

			SaveLoad.Save();
		}
		else
		{
			Debug.Log( string.Format( "InAppProcessor::ProcessPurchase: FAIL. Unrecognized product: '{0}'", productIdentifier ) );
		}
	}

	public Dictionary<string, ProductParam> ProductParamMap
	{
		get
		{
			return m_ProductParamMap;
		}
	}
}
