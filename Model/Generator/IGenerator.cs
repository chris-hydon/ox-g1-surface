using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurfaceTower.Model.Generator
{
  public interface IGenerator
  {
    bool Done { get; }
    void Generate();
  }
}
