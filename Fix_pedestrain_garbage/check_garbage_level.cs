using System;
using ICities;
using UnityEngine;
using ColossalFramework;

namespace Fix_pedestrain_garbage
{
    public class CheckGarbageLevel : ThreadingExtensionBase
    {


        private readonly BuildingManager _buildingManager;
        private readonly SimulationManager _simulationManager;
        private readonly DistrictManager _districtManager;

        public CheckGarbageLevel()
        {
            this._buildingManager = Singleton<BuildingManager>.instance;
            this._simulationManager = Singleton<SimulationManager>.instance;
            this._districtManager = Singleton<DistrictManager>.instance;
            
        }

        public static ushort mostgarbage = 0;

        //run on every simulation tick. from the class ThreadingExtensionBase
        public override void OnAfterSimulationTick()
        {
            ushort pmaxgarbage = mostgarbage;

            if (Fix_pedestrain_garbage.removeAllGarbage||Fix_pedestrain_garbage.removeFromPedestrianArea) { 
            //loop over all buildings over the course of 1000 ticks
                for (var i = (ushort)(_simulationManager.m_currentTickIndex % 1000); i < _buildingManager.m_buildings.m_buffer.Length; i += 1000)
                {
                    //get the id of the "park distric(area type)" the building is in
                    byte parkDistrict = _districtManager.GetPark(_buildingManager.m_buildings.m_buffer[i].m_position, 5, DistrictPark.ParkType.PedestrianZone);
                    //if the id is not 0 then it is in a pedestrian area
                    if (Fix_pedestrain_garbage.removeAllGarbage || (Fix_pedestrain_garbage.removeFromPedestrianArea && parkDistrict != 0))
                    {
                        if(_buildingManager.m_buildings.m_buffer[i].m_garbageBuffer >= Fix_pedestrain_garbage.maxGarbageValue)
                        _buildingManager.m_buildings.m_buffer[i].m_garbageBuffer = 0;
                    }

                    if(_buildingManager.m_buildings.m_buffer[i].m_garbageBuffer> mostgarbage)
                    {
                        mostgarbage = _buildingManager.m_buildings.m_buffer[i].m_garbageBuffer;
                    }
                }
            }

            if(mostgarbage!= pmaxgarbage)
            {
               Console.WriteLine("new max garbage ammount: " + mostgarbage);
            }
            
        }
    
    }
}
