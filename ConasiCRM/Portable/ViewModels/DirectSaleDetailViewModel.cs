using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telerik.XamarinForms.DataControls.TreeView.Commands;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class DirectSaleDetailViewModel : BaseViewModel
    {
        public ICommand LoadOnDemandCommand
        {
            get; set;
        }

        public Guid ProjectId { get; set; }
        public Guid PhasesLanchId { get; set; }
        public bool IsEvent { get; set; }
        public string UnitCode { get; set; }
        public ObservableCollection<string> Directions { get; set; }
        public ObservableCollection<string> Views { get; set; }
        public ObservableCollection<string> UnitStatuses { get; set; }
        public decimal? minNetArea { get; set; }
        public decimal? maxNetArea { get; set; }
        public decimal? minPrice { get; set; }
        public decimal? maxPrice { get; set; }

        public ObservableCollection<Block> Blocks { get; set; }

        public DirectSaleDetailViewModel(DirectSaleSearchModel model)
        {
            IsBusy = true;
            this.ProjectId = model.ProjectId;
            this.PhasesLanchId = model.PhasesLanchId;
            this.IsEvent = model.IsEvent;
            this.UnitCode = model.UnitCode;
            this.Directions = model.Directions;
            this.Views = model.Views;
            this.UnitStatuses = model.UnitStatuses;
            this.minNetArea = model.minNetArea;
            this.maxNetArea = model.maxNetArea;
            this.minPrice = model.minPrice;
            this.maxPrice = model.maxPrice;
            Blocks = new ObservableCollection<Block>();

            AsyncHelper.RunSync(() => this.LoadBlocks());
            //this.LoadOnDemandCommand = new Command(async (p) => await this.LoadOnDemandExecute(p), (p) => this.IsLoadOnDemandEnabled(p));
        }


        /// <summary>
        ///  tesst
        /// </summary>
        /// <param name="model"></param>
        /// <param name="a"></param>
        public DirectSaleDetailViewModel(DirectSaleSearchModel model, int a)
        {
            IsBusy = true;
            this.ProjectId = model.ProjectId;
            this.PhasesLanchId = model.PhasesLanchId;
            this.IsEvent = model.IsEvent;
            Blocks = new ObservableCollection<Block>();
        }

        public async Task LoadBlocks()
        {
            string PhasesLaunch_Condition = (PhasesLanchId != Guid.Empty)
                ? @"<condition attribute='bsd_phaseslaunchid' operator='eq' uitype='bsd_phaseslaunch' value='" + this.PhasesLanchId + @"' />"
                : "";
            //string IsEvent_Condition = (IsEvent)
            //    ? @"<link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaseslaunchid' link-type='inner' alias='ae'>
            //          <link-entity name='bsd_event' from='bsd_phaselaunch' to='bsd_phaseslaunchid' link-type='inner' alias='af' />
            //        </link-entity>"
            //    : "";

            //string xml = xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
            //                  <entity name='product'>
            //                    <attribute name='productid' />
            //                    <attribute name='name' />
            //                    <attribute name='statuscode' />
            //                    <attribute name='bsd_totalprice' />
            //                    <attribute name='bsd_netsaleablearea' />
            //                    <order attribute='bsd_blocknumber' descending='false' />
            //                    <filter type='and'>
            //                        <condition attribute='bsd_projectcode' operator='eq' uitype='bsd_project' value='" + this.ProjectId + @"' />
            //                        " + PhasesLaunch_Condition + @"
            //                    </filter>
            //                    " + IsEvent_Condition + @"
            //                    <link-entity name='bsd_floor' from='bsd_floorid' to='bsd_floor' link-type='inner' alias='ad'>
            //                      <attribute name='bsd_floorid' alias='floorid' />
            //                      <attribute name='bsd_name' alias='floor_name' />
            //                      <filter type='and'>
            //                        <condition attribute='bsd_project' operator='eq' uitype='bsd_project' value='" + this.ProjectId + @"' />
            //                      </filter>
            //                    </link-entity>
            //                    <link-entity name='bsd_block' from='bsd_blockid' to='bsd_blocknumber' link-type='inner' alias='ab'>
            //                      <attribute name='bsd_blockid' alias='blockid' />
            //                      <attribute name='bsd_name' alias='block_name' />
            //                      <filter type='and'>
            //                        <condition attribute='bsd_project' operator='eq' uitype='bsd_project' value='" + this.ProjectId + @"' />
            //                      </filter>
            //                    </link-entity>
            //                  </entity>
            //                </fetch>";

            string IsEvent_Condition = IsEvent ? @"<condition entityname='af' attribute='bsd_eventid' operator='not-null'/>" : "";


            string UnitCode_Condition = !string.IsNullOrEmpty(UnitCode) ? "<condition attribute='name' operator='like' value='%" + UnitCode + "%' />" : "";

            string Direction_Condition = "";
            if(Directions.Count != 0)
            {
                string tmp = "";
                foreach(var i in Directions)
                {
                    tmp += "<value>" + i + "</value>";
                }
                Direction_Condition = @"<condition attribute='bsd_direction' operator='in'>" +
                                            tmp +
                                          "</condition>";
            }


            string View_Condition = "";
            if (Views.Count != 0)
            {
                string tmp = "";
                foreach (var i in Views)
                {
                    tmp += "<value>" + i + "</value>";
                }
                View_Condition = @"<condition attribute='bsd_view' operator='in'>" +
                                            tmp +
                                          "</condition>";
            }


            string UnitStatus_Condition = "";
            if (UnitStatuses.Count != 0)
            {
                string tmp = "";
                foreach (var i in UnitStatuses)
                {
                    tmp += "<value>" + i + "</value>";
                }
                UnitStatus_Condition = @"<condition attribute='statuscode' operator='in'>" +
                                            tmp +
                                          "</condition>";
            }

            string minNetArea_Condition = minNetArea.HasValue ? "<condition attribute='bsd_netsaleablearea' operator='ge' value='" + minNetArea + "' />" : "";
            string maxNetArea_Condition = maxNetArea.HasValue ? "<condition attribute='bsd_netsaleablearea' operator='le' value='" + maxNetArea + "' />" : "";

            string minPrice_Condition = minPrice.HasValue ? "<condition attribute='price' operator='ge' value='" + minPrice + "' />" : "";
            string maxPrice_Condition = maxPrice.HasValue ? "<condition attribute='price' operator='le' value='" + maxPrice + "' />" : "";

            string xml = xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='product'>
                                <attribute name='productid' />
                                <attribute name='name' />
                                <attribute name='statuscode' />
                                <attribute name='bsd_totalprice' />
                                <attribute name='price' />
                                <attribute name='bsd_netsaleablearea' />
                                <order attribute='bsd_blocknumber' descending='false' />
                                <filter type='and'>
                                    <condition attribute='bsd_projectcode' operator='eq' uitype='bsd_project' value='" + this.ProjectId + @"' />
                                    " + PhasesLaunch_Condition + @"
                                    " + IsEvent_Condition + @"
                                    " + UnitCode_Condition + @"
                                    " + Direction_Condition + @"
                                    " + View_Condition + @"
                                    " + UnitStatus_Condition + @"
                                    " + minNetArea_Condition + @"
                                    " + maxNetArea_Condition + @"
                                    " + minPrice_Condition + @"
                                    " + maxPrice_Condition + @"
                                </filter>
                                <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaseslaunchid' link-type='outer' alias='ae'>
                                  <link-entity name='bsd_event' from='bsd_phaselaunch' to='bsd_phaseslaunchid' link-type='outer' alias='af'>
                                      <attribute name='bsd_eventid' alias='event_id' />                                       
                                   </link-entity>
                                </link-entity>
                                <link-entity name='bsd_floor' from='bsd_floorid' to='bsd_floor' link-type='inner' alias='ad'>
                                  <attribute name='bsd_floorid' alias='floorid' />
                                  <attribute name='bsd_name' alias='floor_name' />
                                  <filter type='and'>
                                    <condition attribute='bsd_project' operator='eq' uitype='bsd_project' value='" + this.ProjectId + @"' />
                                  </filter>
                                </link-entity>
                                <link-entity name='bsd_block' from='bsd_blockid' to='bsd_blocknumber' link-type='inner' alias='ab'>
                                  <attribute name='bsd_blockid' alias='blockid' />
                                  <attribute name='bsd_name' alias='block_name' />
                                  <filter type='and'>
                                    <condition attribute='bsd_project' operator='eq' uitype='bsd_project' value='" + this.ProjectId + @"' />
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";
            var unit_result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Unit>>("products", xml);

            var unit_list = unit_result.value;
            List<Unit> block_distinct = unit_list.GroupBy(i => new
            {
                blockid = i.blockid
            }).Select(group => group.First()).ToList();

            var blockCount = block_distinct.Count;
            for (int i = 0; i < blockCount; i++)
            {
                Block b = new Block();
                b.bsd_blockid = block_distinct[i].blockid;
                b.bsd_name = block_distinct[i].block_name;
                CountUnit blockCountUnit = new CountUnit();

                // unit trong block nay
                var unit_in_block = unit_list.Where(x => x.blockid == b.bsd_blockid);

                // tim nhugn unit co cung` floor.
                List<Unit> distinct_by_floor = unit_in_block.GroupBy(u => new
                {
                    Floor = u.floorid
                }).Select(group => group.First()).ToList();


                foreach (var item in distinct_by_floor.OrderBy(x => x.floor_name))
                {
                    Floor floor = new Floor()
                    {
                        bsd_floorid = item.floorid,
                        bsd_name = item.floor_name
                    };

                    //var parentUnit = new Unit();
                    //parentUnit.Items = new List<Unit>();
                    CountUnit floorUnitCount = new CountUnit();
                    var unit_in_floor = unit_in_block.Where(x => x.floorid == item.floorid);
                    foreach (var unit in unit_in_floor)
                    {
                        if (unit.statuscode == 1) // vàng
                        {
                            blockCountUnit.Preparing += 1;
                            floorUnitCount.Preparing += 1;
                        }
                        else if (unit.statuscode == 100000000) // xanh lá cây nhạt
                        {
                            blockCountUnit.Available += 1;
                            floorUnitCount.Available += 1;
                        }
                        else if (unit.statuscode == 100000004)
                        {
                            blockCountUnit.Queueing += 1;
                            floorUnitCount.Queueing += 1;
                        }
                        else if (unit.statuscode == 100000006)// xanh la cay đậm
                        {
                            blockCountUnit.Reserve += 1;
                            floorUnitCount.Reserve += 1;
                        }
                        else if (unit.statuscode == 100000005)
                        {
                            blockCountUnit.Collected += 1;
                            floorUnitCount.Collected += 1;
                        }
                        else if (unit.statuscode == 100000003)
                        {
                            blockCountUnit.Deposited += 1;
                            floorUnitCount.Deposited += 1;
                        }
                        else if (unit.statuscode == 100000009) // Thỏa thuận đặt cọc
                        {
                            blockCountUnit.ThoaThuanDatCoc += 1;
                            floorUnitCount.ThoaThuanDatCoc += 1;
                        }
                        else if (unit.statuscode == 100000001) // 1st Installment
                        {
                            blockCountUnit.StInstallment += 1;
                            floorUnitCount.StInstallment += 1;
                        }
                        else if (unit.statuscode == 100000002)
                        {
                            blockCountUnit.Sold += 1;
                            floorUnitCount.Sold += 1;
                        }
                        floor.Units.Add(unit);
                        //parentUnit.Items.Add(unit);
                    }

                    //floor.Units.Add(parentUnit);
                    floor.CountUnit = floorUnitCount;
                    b.Floors.Add(floor);
                }
                b.CountUnit = blockCountUnit;
                Blocks.Add(b);
            }
        }
        public async Task LoadBlocks2()
        {
            string FetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='bsd_block'>
                <attribute name='bsd_name' />
                <attribute name='bsd_blockid' />
                <order attribute='createdon' descending='true' />
                <filter type='and'>
                  <condition attribute='bsd_project' operator='eq' uitype='bsd_project' value='" + this.ProjectId + @"' />
                </filter>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Block>>("bsd_blocks", FetchXml);
            var blocks = result.value;
            foreach (Block block in blocks)
            {
                string xml = string.Empty;
                if (PhasesLanchId == Guid.Empty)
                {
                    xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='product'>
                        <attribute name='productid' />
                        <attribute name='statuscode' />
                        <order attribute='statuscode' descending='true' />
                        <filter type='and'>
                          <condition attribute='bsd_blocknumber' operator='eq' uitype='bsd_block' value='" + block.bsd_blockid + @"' />
                        </filter>
                        <link-entity name='bsd_floor' from='bsd_floorid' to='bsd_floor' visible='false' link-type='outer' alias='a_4d73a1e06ce2e811a94e000d3a1bc2d1'>
                          <attribute name='bsd_floorid' alias='floorid' />
                          <attribute name='bsd_name' alias='floor_name' />
                        </link-entity>
                      </entity>
                    </fetch>";
                }
                else
                {
                    xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='product'>
                        <attribute name='productid' />
                        <attribute name='statuscode' />
                        <order attribute='statuscode' descending='true' />
                        <filter type='and'>
                          <condition attribute='bsd_blocknumber' operator='eq' uitype='bsd_block' value='" + block.bsd_blockid + @"' />
                          <condition attribute='bsd_phaseslaunchid' operator='eq' uitype='bsd_phaseslaunch' value='" + PhasesLanchId + @"' />
                        </filter>
                        <link-entity name='bsd_floor' from='bsd_floorid' to='bsd_floor' visible='false' link-type='outer' alias='a_4d73a1e06ce2e811a94e000d3a1bc2d1'>
                          <attribute name='bsd_floorid' alias='floorid' />
                          <attribute name='bsd_name' alias='floor_name' />
                        </link-entity>
                      </entity>
                    </fetch>";
                }

                var units_result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Unit>>("products", xml);
                var units = units_result.value;
                int unitCount = units.Count;
                for (int i = 0; i < unitCount; i++)
                {
                    if (units[i].statuscode == 1)
                    {
                        block.CountUnit.Preparing += 1;
                    }
                    else if (units[i].statuscode == 100000000)
                    {
                        block.CountUnit.Available += 1;
                    }
                    else if (units[i].statuscode == 100000004)
                    {
                        block.CountUnit.Queueing += 1;
                    }
                    else if (units[i].statuscode == 100000006)
                    {
                        block.CountUnit.Reserve += 1;
                    }
                    else if (units[i].statuscode == 100000005)
                    {
                        block.CountUnit.Collected += 1;
                    }
                    else if (units[i].statuscode == 100000003)
                    {
                        block.CountUnit.Deposited += 1;
                    }
                    else if (units[i].statuscode == 100000009)
                    {
                        block.CountUnit.ThoaThuanDatCoc += 1;
                    }
                    else if (units[i].statuscode == 100000002)
                    {
                        block.CountUnit.Sold += 1;
                    }
                }

                block.CountUnit.Preparing = units.Count(x => x.statuscode == 1);
                block.CountUnit.Available = units.Count(x => x.statuscode == 100000000);
                block.CountUnit.Queueing = units.Count(x => x.statuscode == 100000004);
                block.CountUnit.Reserve = units.Count(x => x.statuscode == 100000006);
                block.CountUnit.Collected = units.Count(x => x.statuscode == 100000005);
                block.CountUnit.Deposited = units.Count(x => x.statuscode == 100000003);
                block.CountUnit.ThoaThuanDatCoc = units.Count(x => x.statuscode == 100000009);
                block.CountUnit.Sold = units.Count(x => x.statuscode == 100000002);

                Blocks.Add(block);
            }
        }

        private bool IsLoadOnDemandEnabled(object p)
        {
            var context = (TreeViewLoadOnDemandCommandContext)p;
            return context.Item is Block || context.Item is Floor;
        }
        async private Task LoadOnDemandExecute(object p)
        {
            var context = (TreeViewLoadOnDemandCommandContext)p;

            if (context.Item != null && context.Item is Block)
            {
                var block = context.Item as Block;

                string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='product'>
                    <attribute name='name' />
                    <attribute name='statuscode' />
                    <attribute name='bsd_totalprice' />
                    <attribute name='bsd_netsaleablearea' />
                    <attribute name='bsd_floor' />
                    <order attribute='statuscode' descending='true' />
                    <link-entity name='bsd_floor' from='bsd_floorid' to='bsd_floor' link-type='inner' alias='ad'>
                       <attribute name='bsd_name'  alias='floor_name'/>
                      <filter type='and'>
                        <condition attribute='bsd_block' operator='eq' uitype='bsd_block' value='" + block.bsd_blockid + @"' />
                      </filter>
                    </link-entity>
                  </entity>
                </fetch>";

                var productlist_result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Unit>>("products", fetchXml);

                var productlist = productlist_result.value;
                List<Unit> floors_distinct = productlist.GroupBy(i => new
                {
                    floorid = i._bsd_floor_value
                }).Select(group => group.First()).ToList();


                foreach (var unit in floors_distinct)
                {
                    var floorid = unit._bsd_floor_value;

                    Floor floor = new Floor();
                    floor.bsd_floorid = floorid;
                    floor.bsd_name = unit.floor_name;

                    var units = productlist.Where(x => x._bsd_floor_value == floorid).ToList();
                    int unitCount = units.Count;
                    for (int i = 0; i < unitCount; i++)
                    {
                        //floor.Units.Add(units[i]);
                        if (units[i].statuscode == 1)
                        {
                            floor.CountUnit.Preparing += 1;
                        }
                        else if (units[i].statuscode == 100000000)
                        {
                            floor.CountUnit.Available += 1;
                        }
                        else if (units[i].statuscode == 100000004)
                        {
                            floor.CountUnit.Queueing += 1;
                        }
                        else if (units[i].statuscode == 100000006)
                        {
                            floor.CountUnit.Reserve += 1;
                        }
                        else if (units[i].statuscode == 100000005)
                        {
                            floor.CountUnit.Collected += 1;
                        }
                        else if (units[i].statuscode == 100000003)
                        {
                            floor.CountUnit.Deposited += 1;
                        }
                        else if (units[i].statuscode == 100000002)
                        {
                            floor.CountUnit.Sold += 1;
                        }
                    }

                    block.Floors.Add(floor);
                }

                //var floorService = new CRMService<Floor>();
                //var floors = await floorService.RetrieveMultiple("bsd_floors", @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                //      <entity name='bsd_floor'>
                //        <attribute name='bsd_name' />
                //        <attribute name='createdon' />
                //        <attribute name='statuscode' />
                //        <attribute name='bsd_release' />
                //        <attribute name='bsd_project' />
                //        <attribute name='bsd_floor' />
                //        <attribute name='bsd_block' />
                //        <attribute name='bsd_floorid' />
                //        <order attribute='createdon' descending='true' />
                //        <filter type='and'>
                //          <condition attribute='bsd_block' operator='eq' uitype='bsd_block' value='" + block.bsd_blockid + @"' />
                //        </filter>
                //      </entity>
                //    </fetch>");
                //var countFloor = floors.Count;
                //for (int f = 0; f < countFloor; f++)
                //{
                //    var floor = floors[f];
                //    string xml = string.Empty;
                //    if (PhasesLanchId == Guid.Empty)
                //    {
                //        xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                //          <entity name='product'>
                //            <attribute name='statuscode' />
                //            <order attribute='statuscode' descending='true' />
                //            <filter type='and'>
                //              <condition attribute='bsd_floor' operator='eq' uitype='bsd_floor' value='" + floor.bsd_floorid + @"' />
                //            </filter>
                //          </entity>
                //        </fetch>";
                //    }
                //    else
                //    {
                //        xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                //          <entity name='product'>
                //            <attribute name='statuscode' />
                //            <order attribute='statuscode' descending='true' />
                //            <filter type='and'>
                //              <condition attribute='bsd_floor' operator='eq' uitype='bsd_floor' value='" + floor.bsd_floorid + @"' />
                //              <condition attribute='bsd_phaseslaunchid' operator='eq' uitype='bsd_phaseslaunch' value='" + PhasesLanchId + @"' />
                //            </filter>
                //          </entity>
                //        </fetch>";
                //    }

                //    var units = await floorService.DynamicRetrieve<Unit>("products", xml);
                //    int unitCount = units.Count;
                //    for (int i = 0; i < unitCount; i++)
                //    {
                //        if (units[i].statuscode == 1)
                //        {
                //            floor.CountUnit.Preparing += 1;
                //        }
                //        else if (units[i].statuscode == 100000000)
                //        {
                //            floor.CountUnit.Available += 1;
                //        }
                //        else if (units[i].statuscode == 100000004)
                //        {
                //            floor.CountUnit.Queueing += 1;
                //        }
                //        else if (units[i].statuscode == 100000006)
                //        {
                //            floor.CountUnit.Reserve += 1;
                //        }
                //        else if (units[i].statuscode == 100000005)
                //        {
                //            floor.CountUnit.Collected += 1;
                //        }
                //        else if (units[i].statuscode == 100000003)
                //        {
                //            floor.CountUnit.Deposited += 1;
                //        }
                //        else if (units[i].statuscode == 100000002)
                //        {
                //            floor.CountUnit.Sold += 1;
                //        }
                //    }

                //    //floor.CountUnit.Preparing = units.Count(x => x.statuscode == 1);
                //    //floor.CountUnit.Available = units.Count(x => x.statuscode == 100000000);
                //    //floor.CountUnit.Queueing = units.Count(x => x.statuscode == 100000004);
                //    //floor.CountUnit.Reserve = units.Count(x => x.statuscode == 100000006);
                //    //floor.CountUnit.Collected = units.Count(x => x.statuscode == 100000005);
                //    //floor.CountUnit.Deposited = units.Count(x => x.statuscode == 100000003);
                //    //floor.CountUnit.Sold = units.Count(x => x.statuscode == 100000002);


                //    block.Floors.Add(floor);
                //}
                context.Finish();
            }
            else if (context.Item != null && context.Item is Floor)
            {
                var floor = context.Item as Floor;
                floor.Tests.Add(new Test());
                var units_result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Unit>>("products", @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='product'>
                        <attribute name='productid' />
                        <attribute name='name' />
                        <attribute name='statuscode' />
                        <attribute name='bsd_totalprice' />
                        <attribute name='bsd_netsaleablearea' />
                        <attribute name='createdon' />                        
                        <order attribute='createdon' descending='true' />
                        <filter type='and'>
                          <condition attribute='bsd_floor' operator='eq' uitype='bsd_floor' value='" + floor.bsd_floorid + @"' />
                        </filter>
                      </entity>
                    </fetch>");
                var units = units_result.value;
                foreach (var item in units)
                {
                    floor.Units.Add(item);
                    //floor.Tests.FirstOrDefault().Units.Add(item);
                }
                context.Finish();
            }
        }
    }
}
