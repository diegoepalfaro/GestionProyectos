using System;
using System.Collections.Generic;

namespace GestionTareas.Models;

public partial class MigrationHistory
{
    public string MigrationID { get; set; } = null!;

    public string ContextKey { get; set; } = null!;

    public byte[] Model { get; set; } = null!;

    public string ProductVersion { get; set; } = null!;
}
