using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurfaceTower.Model.Generator
{
  //Any Generator muse be able to signal that it has expired (Done), and generate enemies when prompted.
  public interface IGenerator
  {
    bool Done { get; }
    void Generate();
  }
}
