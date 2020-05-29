using component.helper;
using component.logger;
using iot.solution.data;
using iot.solution.entity;
using iot.solution.model.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Entity = iot.solution.entity;
using Model = iot.solution.model.Models;


namespace iot.solution.model.Repository.Implementation
{
    public class EntityRepository : GenericRepository<Model.Entity>, IEntityRepository
    {
        private readonly ILogger logger;
        public EntityRepository(IUnitOfWork unitOfWork, ILogger logManager) : base(unitOfWork, logManager)
        {
            logger = logManager;
            _uow = unitOfWork;
        }
        public List<Entity.LookupItem> GetLookup(Guid companyId)
        {
            var result = new List<Entity.LookupItem>();
            try
            {
                logger.Information(Constants.ACTION_ENTRY, "EntityRepository.GetLookup");
                result = _uow.DbContext.Entity.Where(u => u.CompanyGuid.Equals(companyId)  && u.ParentEntityGuid.Equals(SolutionConfiguration.EntityGuid) && u.IsActive == true && !u.IsDeleted).Select(g => new Entity.LookupItem() { Text = g.Name, Value = g.Guid.ToString() }).ToList();
                logger.Information(Constants.ACTION_EXIT, "EntityRepository.GetLookup");
            }
            catch (Exception ex)
            {
                logger.Error(Constants.ACTION_EXCEPTION, ex);
            }
            return result;
        }
        public List<Entity.LookupItem> GetZoneLookup(Guid companyId)
        {
            var result = new List<Entity.LookupItem>();
            try
            {
                logger.Information(Constants.ACTION_ENTRY, "EntityRepository.GetZoneLookup");
                result = _uow.DbContext.Entity.Where(u => u.CompanyGuid.Equals(companyId) && !u.ParentEntityGuid.Equals(SolutionConfiguration.EntityGuid) && u.IsActive == true && !u.IsDeleted).Select(g => new Entity.LookupItem() { Text = g.Name, Value = g.Guid.ToString() }).ToList();
                logger.Information(Constants.ACTION_EXIT, "EntityRepository.GetZoneLookup");
            }
            catch (Exception ex)
            {
                logger.Error(Constants.ACTION_EXCEPTION, ex);
            }
            return result;
        }
        public Entity.SearchResult<List<Entity.EntityDetail>> List(Entity.SearchRequest request)
        {
            Entity.SearchResult<List<Entity.EntityDetail>> result = new Entity.SearchResult<List<Entity.EntityDetail>>();
            try
            {
                logger.Information(Constants.ACTION_ENTRY, "EntityRepository.List");
                using (var sqlDataAccess = new SqlDataAccess(ConnectionString))
                {
                    List<DbParameter> parameters = sqlDataAccess.CreateParams(component.helper.SolutionConfiguration.CurrentUserId, request.Version);
                    parameters.Add(sqlDataAccess.CreateParameter("companyguid", component.helper.SolutionConfiguration.CompanyId, DbType.Guid, ParameterDirection.Input));
                    //if (!request.EntityId.Equals(Guid.Empty) && !request.EntityId.Equals(SolutionConfiguration.EntityGuid))
                    //{
                    //    parameters.Add(sqlDataAccess.CreateParameter("parentEntityGuid", request.EntityId, DbType.Guid, ParameterDirection.Input));
                    //}
                    parameters.Add(sqlDataAccess.CreateParameter("search", request.SearchText, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("pagesize", request.PageSize, DbType.Int32, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("pagenumber", request.PageNumber, DbType.Int32, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("orderby", request.OrderBy, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("count", DbType.Int32, ParameterDirection.Output, 16));
                    DbDataReader dbDataReader = sqlDataAccess.ExecuteReader(sqlDataAccess.CreateCommand("[Entity_List]", CommandType.StoredProcedure, null), parameters.ToArray());
                    result.Items = DataUtils.DataReaderToList<Entity.EntityDetail>(dbDataReader, null);
                    result.Count = int.Parse(parameters.Where(p => p.ParameterName.Equals("count")).FirstOrDefault().Value.ToString());


                    if(result.Count > 0)
                    {
                        foreach (var item in result.Items)
                        {
                            if(!string.IsNullOrEmpty(item.Attributes))
                                item.AttributeList = DeserlizeXML(item.Attributes);
                            item.Attributes = "";
                        }
                    }
                }
                logger.Information(Constants.ACTION_EXIT, "EntityRepository.List");
            }
            catch (Exception ex)
            {
                logger.Error(Constants.ACTION_EXCEPTION, ex);
            }
            return result;
        }
        public ActionStatus Manage(Model.Entity request)
        {
            ActionStatus result = new ActionStatus(true);
            try
            {
                logger.Information(Constants.ACTION_ENTRY, "EntityRepository.Manage");
                using (var sqlDataAccess = new SqlDataAccess(ConnectionString))
                {
                    List<DbParameter> parameters = sqlDataAccess.CreateParams(component.helper.SolutionConfiguration.CurrentUserId, component.helper.SolutionConfiguration.Version);
                    
                    parameters.Add(sqlDataAccess.CreateParameter("companyGuid", request.CompanyGuid, DbType.Guid, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("guid", request.Guid, DbType.Guid, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("parentEntityGuid", request.ParentEntityGuid, DbType.Guid, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("name", request.Name, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("type", request.Type, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("description", request.Description, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("latitude", request.Latitude, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("longitude ", request.Longitude, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("address", request.Address, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("address2", request.Address2, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("city", request.City, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("stateGuid", request.StateGuid, DbType.Guid, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("countryGuid", request.CountryGuid, DbType.Guid, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("zipCode", request.Zipcode, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("image", request.Image, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("newid", request.Guid,  DbType.Guid, ParameterDirection.Output));
                    parameters.Add(sqlDataAccess.CreateParameter("culture", component.helper.SolutionConfiguration.Culture, DbType.String, ParameterDirection.Input));
                    parameters.Add(sqlDataAccess.CreateParameter("enableDebugInfo", component.helper.SolutionConfiguration.EnableDebugInfo, DbType.String, ParameterDirection.Input));
                    int intResult = sqlDataAccess.ExecuteNonQuery(sqlDataAccess.CreateCommand("[Entity_AddUpdate]", CommandType.StoredProcedure, null), parameters.ToArray());                    
                    
                    int outPut = int.Parse(parameters.Where(p => p.ParameterName.Equals("output")).FirstOrDefault().Value.ToString());
                    if (outPut > 0)
                    {
                        string guidResult = parameters.Where(p => p.ParameterName.Equals("newid")).FirstOrDefault().Value.ToString();
                        if (!string.IsNullOrEmpty(guidResult))
                        {
                            result.Data = _uow.DbContext.Entity.Where(u => u.Guid.Equals(Guid.Parse(guidResult))).FirstOrDefault();
                        }
                    }
                    else
                    {
                        result.Message = parameters.Where(p => p.ParameterName.Equals("fieldname")).FirstOrDefault().Value.ToString();
                    }
                }
                logger.Information(Constants.ACTION_EXIT, "EntityRepository.Manage");
            }
            catch (Exception ex)
            {
                logger.Error(Constants.ACTION_EXCEPTION, ex);
            }
            return result;
        }


        private List<entity.AttributeXMLResponse> DeserlizeXML(string xml)
        {
            try
            {
               // xml = "<attributes><attribute><key>ag</key><value>11.285000</value></attribute><attribute><key>bf4</key><value>0.000000</value></attribute><attribute><key>br</key><value>11.130000</value></attribute><attribute><key>ca2</key><value>40.040000</value></attribute><attribute><key>cl</key><value>54.855000</value></attribute><attribute><key>clo4</key><value>0.000000</value></attribute><attribute><key>conv</key><value>20.545000</value></attribute><attribute><key>cu2</key><value>8.430000</value></attribute><attribute><key>do</key><value>10.170000</value></attribute><attribute><key>f</key><value>35.150000</value></attribute><attribute><key>k</key><value>0.000000</value></attribute><attribute><key>li</key><value>0.000000</value></attribute><attribute><key>lod</key><value>5.825000</value></attribute><attribute><key>mg2</key><value>0.000000</value></attribute><attribute><key>n03</key><value>45.165000</value></attribute><attribute><key>na</key><value>0.000000</value></attribute><attribute><key>nh4</key><value>0.000000</value></attribute><attribute><key>no2</key><value>0.000000</value></attribute><attribute><key>orp</key><value>14.635000</value></attribute><attribute><key>ph</key><value>4.915000</value></attribute><attribute><key>temp</key><value>30.070000</value></attribute><attribute><key>turbidity</key><value>24.890000</value></attribute><attribute><key>consumption</key><value>0.000000</value></attribute></attributes>";

                var serializer = new XmlSerializer(typeof(AttributeXMLData), new XmlRootAttribute("attributes"));
                using (var stringReader = new StringReader(xml))
                using (var reader = XmlReader.Create(stringReader))
                {
                    var result = (AttributeXMLData)serializer.Deserialize(reader);

                    return result.AttributeList;
                   
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
       
    }
   

    
}
