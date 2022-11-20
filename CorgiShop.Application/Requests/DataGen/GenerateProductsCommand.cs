﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgiShop.Application.Requests.DataGen
{
    public record GenerateProductsCommand(int NumberToGenerate) : IRequest;
}