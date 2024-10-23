namespace Backend.CrossCuting.Helpers.Enum
{
    public class ReturnMessage
    {
        public const string Ok = "";
        public const string OkError = "Se encontró un error al validar la información";
        public const string NoUser = "Credenciales inválidas";
        public const string Delete = "Se eliminó correctamente el registro";
        public const string DeleteError = "No se puede eliminar el registro";
        public const string Get = "Validación Correcta";
        public const string GetError = "Validación Incorrecta";
        public const string Patch = "Se actualizó el estado correctamente";
        public const string PatchError = "No se puede actualizar el estado";
        public const string Post = "Se agregó correctamente el registro";
        public const string PostError = "No se puede agregar el registro";
        public const string Put = "Se editó correctamente el registro";
        public const string PutError = "No se puede editar el registro";
    }
}
