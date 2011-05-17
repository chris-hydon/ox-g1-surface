using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurfaceTower.Model
{
  public interface IGenerator
  {
    bool Done { get; }
    void Generate(int number);
  }
}
