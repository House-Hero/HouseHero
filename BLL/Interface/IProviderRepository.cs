﻿using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IProviderRepository:IGenericRepository<Provider> 
    {
        public Provider GetProviderWithAllRelatedData(int ProviderId);
    }
}