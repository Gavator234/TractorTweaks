﻿//=============================================================================
// Adjust LOD settings.
//=============================================================================

public class cmk_Lod : cmk.NMS.Script.ModClass
{
	protected override void Execute()
	{
		GcEnvironmentGlobals();
		GcBuildingGlobals();
		GcGraphicsGlobals();
		TkAttachmentData();
		TkVoxelGeneratorSettingsArray();
	}

	//...........................................................

	protected void GcEnvironmentGlobals()
	{
		var mbin = ExtractMbin<GcEnvironmentGlobals>(
			"GCENVIRONMENTGLOBALS.GLOBAL.MBIN"
		);
		var scale = 0.2f;
		mbin.TerrainFadeTime       *= scale;
		mbin.TerrainFadeTimeInShip *= scale;
		mbin.CreatureFadeTime      *= scale;
		mbin.FloraFadeTimeMin      *= scale;
		mbin.FloraFadeTimeMax      *= scale;
	}

	//...........................................................

	protected void GcBuildingGlobals()
	{
		var mbin = ExtractMbin<GcBuildingGlobals>(
			"GCBUILDINGGLOBALS.GLOBAL.MBIN"
		);
		var scale = 10.0f;
		foreach( var distance in mbin.ActiveLodDistances ) {
			for( var i = 0; i < distance.Distances.Length; ++i ) {
				distance.Distances[i] *= scale;
			}
		}
		foreach( var distance in mbin.InactiveLodDistances ) {
			for( var i = 0; i < distance.Distances.Length; ++i ) {
				distance.Distances[i] *= scale * 2;
			}
		}
		mbin.FadeDistance *= 10;
	}

	//...........................................................

	protected void GcGraphicsGlobals()
	{
		var mbin = ExtractMbin<GcGraphicsGlobals>(
			"GCGRAPHICSGLOBALS.GLOBAL.MBIN"
		);
		
	//	mbin.ForceUncachedTerrain = true;  // it's what all the cool kids are doing
					
		mbin.TerrainMipDistanceHigh = new nms.Vector4f { x =  0, y =  64, z = 128, t = 256 };
		mbin.TerrainMipDistanceUlt  = new nms.Vector4f { x =  0, y = 128, z = 256, t = 512 };

		mbin.TerrainDroppedMipsLow = 0;
		mbin.TerrainDroppedMipsMed = 0;
		mbin.TerrainMipBiasLow     = 0;
		
		var scale = 4.0;  // or make all int.MaxValue
		mbin.TerrainBlocksPerFrameLow = (int)(scale * mbin.TerrainBlocksPerFrameLow);  // 16
		mbin.TerrainBlocksPerFrameMed = (int)(scale * mbin.TerrainBlocksPerFrameMed);  // 24
		mbin.TerrainBlocksPerFrameHi  = (int)(scale * mbin.TerrainBlocksPerFrameHi);   // 48
		mbin.TerrainBlocksPerFrameUlt = (int)(scale * mbin.TerrainBlocksPerFrameUlt);  // 64
		
	//	TaaFix(mbin);
	}	

	//...........................................................

	protected void TaaFix( GcGraphicsGlobals MBIN ) 
	{
		// TAA shimmer fix
		MBIN.TaaSettings = new nms.Vector4f { x = 2, y = 0.99f, z = -0.909999967f, t = -10 };
		MBIN.TaaLowFreqConstant  = 100;
		MBIN.TaaHighFreqConstant = 100;
		MBIN.TaaAccumDelay       = 1.05f;
		MBIN.FrustumJitterAmount = 0;
		MBIN.UseTaaResolve       = false;
		MBIN.ApplyTaaTest        = false;
		MBIN.ShowTaaBuf          = false;
		MBIN.ShowTaaVarianceBuf  = false;
		MBIN.ShowTaaNVarianceBuf = false;
		MBIN.ShowTaaCVarianceBuf = false;			
	}
	
	//...........................................................

	protected void TkVoxelGeneratorSettingsArray()
	{
		var mbin = ExtractMbin<TkVoxelGeneratorSettingsArray>(
			"METADATA/SIMULATION/SOLARSYSTEM/VOXELGENERATORSETTINGS.MBIN"
		);
		foreach( var elem in mbin.TerrainSettings ) {
			foreach( var noise in elem.Min.NoiseLayers ) {
				noise.MaximumLOD = noise.MaximumLOD + 2;
			}
			foreach( var noise in elem.Max.NoiseLayers ) {
				noise.MaximumLOD = noise.MaximumLOD + 2;
			}
		}
	}		

	//...........................................................
	
	// changes ~2,000 entity mbin's
	protected void TkAttachmentData()
	{
		var scale    = 10.0f;
		var entities = Game.Mbinc.FindClass("TkAttachmentData"); //finds all mbins with this metadata
		foreach( var path in entities.PakItems ) {
			//	Unless it's a wall switch, don't change any nintendo switch assets, that would just bloat it.
			if (path.Contains("SWITCH")) {
				if (!path.Contains("UTILITYPARTS/SWITCH"))
					continue;
			}
			var mbin = ExtractMbin<TkAttachmentData>(path, true, false);
			for( var i = 0; i < mbin.LodDistances.Length; ++i ) {
				mbin.LodDistances[i] *= scale;
			}
		}
	}	
}

//=============================================================================
