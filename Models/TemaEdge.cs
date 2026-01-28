namespace Quibee.Models
{
    /// <summary>
    /// Representa una conexión (arista) entre dos nodos del mapa de lecciones.
    /// Define la dirección From → To para dibujar líneas conectoras.
    /// </summary>
    public class TemaEdge
    {
        /// <summary>
        /// ID del nodo de origen
        /// </summary>
        public string FromId { get; set; } = string.Empty;

        /// <summary>
        /// ID del nodo de destino
        /// </summary>
        public string ToId { get; set; } = string.Empty;

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public TemaEdge()
        {
        }

        /// <summary>
        /// Constructor con parámetros
        /// </summary>
        /// <param name="fromId">ID del nodo origen</param>
        /// <param name="toId">ID del nodo destino</param>
        public TemaEdge(string fromId, string toId)
        {
            FromId = fromId;
            ToId = toId;
        }
    }
}
