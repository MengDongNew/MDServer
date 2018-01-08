using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Modal;
using Common.Tool;
using GameServerApplication.DB.Manager;
using MDServer.GameServer;

namespace GameServerApplication.Handlers
{
    class AccountHandler : HandlerBase
    {
        private readonly UserManager manager = new UserManager();
        #region interface
        public override OperationCode OpCode { get { return OperationCode.Account; } }

        public override void OnHandleMessage(OperationRequest request, OperationResponse response, MasterClientPeer peer, SendParameters sendParameters)
        {
            var user = ParameterTool.GetParameter<User>(request.Parameters, ParameterCode.User);
            var userDB = manager.GetUserByAccesstoken(user.Accesstoken);

            SubCode subCode = ParameterTool.GetSubcode(request.Parameters);
            ParameterTool.AddSubcode(response.Parameters, subCode);
            switch (subCode) {
                case SubCode.AccLogin:
                    {
                        if (userDB != null && userDB.Password == MD5Tool.GetMD5(user.Password))
                        {
                            ParameterTool.AddParameter<User>(response.Parameters, ParameterCode.User, userDB);
                            //用户名和密码正确，登陆成功
                            response.ReturnCode = (short)ReturnCode.Success;
                            peer.User = userDB;


                            //if (MasterApplication.Instance.TeamCtrl.ContansUser(userDB.ID))//如果之前正在游戏中
                            //{
                            //    peer.Team = MasterApplication.Instance.TeamCtrl.GetTeamByUser(userDB.ID);
                            //}
                        }
                        else
                        {
                            response.ReturnCode = (short)ReturnCode.Fail;
                            response.DebugMessage = "用户名或密码错误";
                        }
                        //string username = ParameterTool.GetParameter<string>(request.Parameters, ParameterCode.Username, false);
                        // password = ParameterTool.GetParameter<string>(request.Parameters, ParameterCode.Password, false);
                        Log("Login:acesstoken="+user.Accesstoken+",password="+user.Password);
                    } break;
                case SubCode.AccRegister:
                    {
                        if (userDB == null)
                        {
                            user.Password = MD5Tool.GetMD5(user.Password);
                            user.Username = user.Accesstoken;
                            manager.AddUser(user);
                            response.ReturnCode = (short)ReturnCode.Success;
                        }
                        else
                        {
                            response.ReturnCode = (short)ReturnCode.Fail;
                            response.DebugMessage = "用户已存在";
                        }
                        //string username = ParameterTool.GetParameter<string>(request.Parameters, ParameterCode.Username, false);
                        //string password = ParameterTool.GetParameter<string>(request.Parameters, ParameterCode.Password, false);
                        Log("Register:acesstoken=" + user.Accesstoken + ",password=" + user.Password);
                    } break;
            }
        }
        #endregion
        void Log(string s)
        {
            Console.WriteLine("AccountHandler:" + s);
        }
    }
}
