﻿using ATD2016.ApplicationParts.Models;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ATD2016.ApplicationParts.GenericController
{
    // An IApplicationFeatureProvider<ControllerFeature> will be used at startup time to discover types
    // that should be treated as controllers. Normally MVC will ignore an open generic type like
    // GenericController<>.
    //
    // This component will create a new type (like GenericController<Widget>) for each entity type
    // that does not already have a non-generic controller defined. We determine this based on the type
    // name. So because SprocketController is defined, we don't add GenericController<Sprocket> as it
    // would create a conflict.
    public class GenericControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            // This is designed to run after the default ControllerTypeProvider, so the list of 'real' controllers
            // has already been populated.
            foreach (var modelType in PublicModelTypes.Types)
            {
                var typeName = modelType.Name + "Controller";
                if (feature.Controllers.Any(t => t.Name == typeName)) continue;

                // There's no 'real' controller for this entity, so add the generic version.
                var controllerType = typeof(GenericController<>).MakeGenericType(modelType.AsType()).GetTypeInfo();

                feature.Controllers.Add(controllerType);
            }
        }
    }
}
