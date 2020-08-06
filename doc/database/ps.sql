/*
Navicat MySQL Data Transfer

Source Server         : ps
Source Server Version : 50505
Source Host           : localhost:3306
Source Database       : ps

Target Server Type    : MYSQL
Target Server Version : 50505
File Encoding         : 65001

Date: 2020-08-06 15:49:37
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `sys_files`
-- ----------------------------
DROP TABLE IF EXISTS `sys_files`;
CREATE TABLE `sys_files` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL COMMENT '图片名称',
  `path` varchar(200) NOT NULL COMMENT '真实路径',
  `length` int(11) NOT NULL COMMENT '长度',
  `upload_time` bigint(20) NOT NULL DEFAULT 0 COMMENT '上传时间',
  `remark` varchar(200) NOT NULL DEFAULT '' COMMENT '备注',
  `url` varchar(254) NOT NULL DEFAULT '' COMMENT '网址',
  `file_type` varchar(50) NOT NULL DEFAULT '' COMMENT '文件类型',
  `md5_str` char(32) NOT NULL DEFAULT '' COMMENT 'md5值',
  `base64_str` text NOT NULL DEFAULT '' COMMENT '小图的base64',
  `is_use` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sys_files
-- ----------------------------

-- ----------------------------
-- Table structure for `sys_login`
-- ----------------------------
DROP TABLE IF EXISTS `sys_login`;
CREATE TABLE `sys_login` (
  `id` int(11) NOT NULL,
  `login_name` varchar(20) NOT NULL DEFAULT '' COMMENT '登录名',
  `password` varchar(255) NOT NULL DEFAULT '' COMMENT '密码',
  `phone_no` varchar(20) NOT NULL DEFAULT '' COMMENT '电话号码',
  `email_addr` varchar(255) NOT NULL DEFAULT '' COMMENT '电子邮件',
  `verify_code` varchar(10) NOT NULL DEFAULT '' COMMENT '验证码',
  `verify_time` bigint(20) NOT NULL DEFAULT 0 COMMENT '验证时间',
  `is_locked` smallint(6) NOT NULL DEFAULT 0 COMMENT '是否锁定',
  `pass_update_date` bigint(20) NOT NULL DEFAULT 0 COMMENT '密码修改时间',
  `locked_reason` varchar(255) NOT NULL DEFAULT '' COMMENT '锁定原因',
  `fail_count` int(11) NOT NULL DEFAULT 0 COMMENT '失败次数',
  `create_time` bigint(20) NOT NULL DEFAULT 0 COMMENT '创建时间',
  `login_count` int(11) NOT NULL DEFAULT 0 COMMENT '登录次数',
  `last_login_time` bigint(20) NOT NULL DEFAULT 0 COMMENT '最后登录时间',
  `last_logout_time` bigint(20) NOT NULL DEFAULT 0 COMMENT '最后退出时间',
  `last_active_time` bigint(20) NOT NULL DEFAULT 0 COMMENT '最后活动时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sys_login
-- ----------------------------
INSERT INTO `sys_login` VALUES ('1', '18180770313', '96E79218965EB72C92A549DD5A330112', '18180770313', '', '', '0', '0', '0', '', '0', '0', '0', '0', '0', '0');

-- ----------------------------
-- Table structure for `sys_login_history`
-- ----------------------------
DROP TABLE IF EXISTS `sys_login_history`;
CREATE TABLE `sys_login_history` (
  `id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL COMMENT '用户ID',
  `login_time` bigint(20) NOT NULL COMMENT '登录时间',
  `login_host` varchar(255) NOT NULL DEFAULT '' COMMENT '登录IP',
  `logout_time` bigint(20) NOT NULL COMMENT '退出时间',
  `login_history_type` int(11) NOT NULL COMMENT '1:正常登录\r\n            2:密码错误\r\n            3:验证码错误\r\n            4:工号锁定',
  `message` varchar(255) NOT NULL DEFAULT '' COMMENT '信息',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sys_login_history
-- ----------------------------

-- ----------------------------
-- Table structure for `sys_module`
-- ----------------------------
DROP TABLE IF EXISTS `sys_module`;
CREATE TABLE `sys_module` (
  `id` int(11) NOT NULL,
  `parent_id` int(11) NOT NULL DEFAULT 0 COMMENT '上级ID',
  `name` varchar(60) NOT NULL DEFAULT '' COMMENT '模块名称',
  `location` varchar(2000) NOT NULL DEFAULT '' COMMENT '地址',
  `code` varchar(20) NOT NULL DEFAULT '' COMMENT '代码',
  `is_debug` smallint(6) NOT NULL DEFAULT 0 COMMENT '是否调试',
  `is_hide` smallint(6) NOT NULL DEFAULT 0 COMMENT '隐藏',
  `show_order` smallint(6) NOT NULL DEFAULT 0 COMMENT '排序',
  `description` varchar(2000) NOT NULL DEFAULT '' COMMENT '描述',
  `image_url` varchar(2000) NOT NULL DEFAULT '' COMMENT '图片地址',
  `desktop_role` varchar(200) NOT NULL DEFAULT '' COMMENT '桌面显示角色',
  `w` int(11) NOT NULL DEFAULT 0 COMMENT '宽',
  `h` int(11) NOT NULL DEFAULT 0 COMMENT '高',
  PRIMARY KEY (`id`),
  KEY `FK_sys_module_REF_module` (`parent_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sys_module
-- ----------------------------
INSERT INTO `sys_module` VALUES ('1', '0', '系统管理', '', 'system', '1', '0', '1', '', 'bulb', '', '0', '0');
INSERT INTO `sys_module` VALUES ('2', '1', '角色管理', '/pages/query/query?code=role', 'role', '1', '0', '3', '123', 'bulb', '3', '1', '2');
INSERT INTO `sys_module` VALUES ('3', '1', '模块管理', '/pages/query/query?code=module', 'module', '0', '0', '2', '', 'bulb', '', '0', '0');
INSERT INTO `sys_module` VALUES ('4', '1', '用户管理', '/pages/query/query?code=user', 'user', '0', '0', '4', '', 'bulb', '', '0', '0');
INSERT INTO `sys_module` VALUES ('5', '1', '查询管理', '/pages/query/list', 'query', '0', '0', '1', '222', 'bulb', '', '0', '0');

-- ----------------------------
-- Table structure for `sys_query`
-- ----------------------------
DROP TABLE IF EXISTS `sys_query`;
CREATE TABLE `sys_query` (
  `id` int(11) NOT NULL,
  `name` varchar(50) NOT NULL DEFAULT '' COMMENT '名称',
  `code` varchar(20) NOT NULL DEFAULT '' COMMENT '代码',
  `auto_load` smallint(6) NOT NULL DEFAULT 0 COMMENT '自动加载',
  `page_size` smallint(6) NOT NULL COMMENT '每页大小',
  `show_checkbox` smallint(6) NOT NULL DEFAULT 0 COMMENT '是否有筛选框',
  `is_debug` smallint(6) NOT NULL DEFAULT 0 COMMENT '调试',
  `query_conf` text NOT NULL DEFAULT '' COMMENT '查询内容',
  `query_cfg_json` text NOT NULL DEFAULT '' COMMENT '配置信息',
  `in_para_json` text NOT NULL DEFAULT '' COMMENT '参数',
  `js_str` text NOT NULL DEFAULT '' COMMENT 'js脚本',
  `rows_btn` text NOT NULL DEFAULT '' COMMENT '行按钮',
  `heard_btn` text NOT NULL DEFAULT '' COMMENT '表头按钮',
  `report_script` text NOT NULL DEFAULT '' COMMENT '报表脚本',
  `charts_cfg` text NOT NULL DEFAULT '' COMMENT '图标',
  `charts_type` varchar(50) NOT NULL DEFAULT '' COMMENT '图表类型',
  `remark` text NOT NULL DEFAULT '' COMMENT '备注',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sys_query
-- ----------------------------
INSERT INTO `sys_query` VALUES ('1', '查询管理模板', 'query', '1', '10', '1', '1', 'select id,\r\n    name,\r\n    code,\r\n    auto_load,\r\n    page_size ,\r\n    show_checkbox,\r\n    is_debug,\r\n    remark\r\nfrom ps.sys_query', '{\n	\"ID\": {\n		\"title\": \"查询ID\",\n		\"type\": \"number\",\n		\"editable\": false\n	},\n	\"NAME\": {\n		\"title\": \"查询名\",\n		\"tooltip\": \"用于查询\",\n		\"type\": \"string\"\n	},\n	\"CODE\": {\n		\"title\": \"代码\",\n		\"type\": \"string\"\n	},\n	\"AUTO_LOAD\": {\n		\"title\": \"自动加载\",\n		\"defaultValue\": 1,\n		\"type\": \"custom\",\n		\"renderComponent\": \"SmartTableFormatValuePage\",\n		\"onComponentInitFunction\": function(instance) {\n			instance.format = (x) => {\n				return x == 0 ? \"否\" : \"是\"\n			}\n		},\n		\"editor\": {\n			\"type\": \"list\",\n			\"config\": {\n				\"list\": [{\n						\"value\": \"1\",\n						\"title\": \"是\"\n					},\n					{\n						\"value\": \"0\",\n						\"title\": \"否\"\n					}\n				]\n			}\n		}\n	},\n	\"PAGE_SIZE\": {\n		\"title\": \"页面大小\",\n		\"type\": \"number\",\n		\"defaultValue\": 10\n	},\n	\"SHOW_CHECKBOX\": {\n		\"title\": \"允许多选\",\n		\"type\": \"custom\",\n		\"renderComponent\": \"SmartTableFormatValuePage\",\n		\"onComponentInitFunction\": function(instance) {\n			instance.format = (x) => {\n				return x == 0 ? \"否\" : \"是\"\n			}\n		},\n		\"defaultValue\": 1,\n		\"editor\": {\n			\"type\": \"list\",\n			\"config\": {\n				\"list\": [{\n						\"value\": \"1\",\n						\"title\": \"是\"\n					},\n					{\n						\"value\": \"0\",\n						\"title\": \"否\"\n					}\n				]\n			}\n		}\n	},\n	\"IS_DEBUG\": {\n		\"title\": \"是否隐藏\",\n		\"editable\": false,\n		\"type\": \"custom\",\n		\"renderComponent\": \"SmartTableFormatValuePage\",\n		\"onComponentInitFunction\": function(instance) {\n			instance.format = (x) => {\n				return x == 0 ? \"否\" : \"是\"\n			}\n		},\n		\"defaultValue\": 1,\n		\"editor\": {\n			\"type\": \"list\",\n			\"config\": {\n				\"list\": [{\n						\"value\": \"1\",\n						\"title\": \"是\"\n					},\n					{\n						\"value\": \"0\",\n						\"title\": \"否\"\n					}\n				]\n			}\n		}\n	},\n	\"FILTR_LEVEL\": {\n		\"title\": \"过滤层级\",\n		\"editable\": false,\n		\"type\": \"number\",\n		\"defaultValue\": 1\n	},\n	\"DESKTOP_ROLE\": {\n		\"title\": \"是否首页显示\",\n		\"editable\": false,\n		\"type\": \"string\"\n	},\n	\"NEW_DATA\": {\n		\"title\": \"输入的时间\",\n		\"editable\": false,\n		\"type\": \"string\"\n	},\n	\"QUERY_CONF\": {\n		\"title\": \"查询脚本\",\n		\"tooltip\": \"能查出数据的SQL\",\n		\"type\": \"string\",\n		\"inputWidth\": 12,\n		\"isTabs\": true,\n		\"hide\": true,\n		\"editor\": {\n			\"type\": \"textarea\"\n		}\n	},\n	\"QUERY_CFG_JSON\": {\n		\"title\": \"列配置信息\",\n		\"tooltip\": \"title:标题,type:类型,inputWidth:列宽(6/12),editable:是否可编辑,editor:编辑配置,isTabs:tabs显示,tooltip:编辑框提示,hide:是否在列表显示\",\n		\"type\": \"string\",\n		\"isTabs\": true,\n		\"hide\": true,\n		\"inputWidth\": 12,\n		\"editor\": {\n			\"type\": \"textarea\"\n		}\n	},\n\n	\"IN_PARA_JSON\": {\n		\"title\": \"传入的参数\",\n		\"type\": \"string\",\n		\"isTabs\": true,\n		\"hide\": true,\n		\"inputWidth\": 12,\n		\"editor\": {\n			\"type\": \"textarea\"\n		}\n	},\n	\"JS_STR\": {\n		\"title\": \"JS脚本\",\n		\"type\": \"string\",\n		\"isTabs\": true,\n		\"hide\": true,\n		\"inputWidth\": 12,\n		\"editor\": {\n			\"type\": \"textarea\"\n		}\n	},\n	\"ROWS_BTN\": {\n		\"title\": \"行按钮\",\n		\"tooltip\": \"目前只支持两个按钮，一个修改，一个删除\",\n		\"isTabs\": true,\n		\"hide\": true,\n		\"type\": \"string\"\n	},\n	\"HEARD_BTN\": {\n		\"title\": \"表头按钮\",\n        \"tooltip\": \"添加事件：nowThis.Add(\'api地址\');批量执行：nowThis.exec(\'api地址\',\'列名\',\'确认提示信息\')；导出所有事件：nowThis.onExportXls()\",\n		\"isTabs\": true,\n		\"hide\": true,\n		\"type\": \"string\"\n	},\n\n	\"REMARK\": {\n		\"title\": \"备注\",\n		\"isTabs\": true,\n		\"hide\": true,\n		\"type\": \"string\",\n		\"inputWidth\": 12,\n		\"editor\": {\n			\"type\": \"textarea\"\n		}\n	}\n}', '', '', '[{\n		\"title\": \"修改\",\n		\"class\": \"nb-edit\",\n		\"apiUrl\": \"query/save\"\n	},\n	{\n		\"title\": \"删除\",\n		\"class\": \"nb-trash\",\n		\"apiUrl\": \"query/delete\",\n		\"confirmTip\": \"确定要删除吗？\"\n	}\n]', '[\n  {\n    \"title\":\"添加\",\n    \"class\":\"nb-plus\",\n    \"click\":\"nowThis.Add(\'query/save\')\"\n  },\n  {\n    \"title\":\"导出\",\n    \"class\":\"ion-archive\",\n    \"click\":\"nowThis.onExportXls()\"\n  }\n  ,\n  {\n    \"title\":\"设置\",\n    \"class\":\"nb-gea\"\n  }\n]\n', '', '', '', '可以管理系统的模块1');
INSERT INTO `sys_query` VALUES ('2', '查询用户', 'user', '1', '10', '1', '1', 'select\r\n  a.id,\r\n  a.name,\r\n  a.login_name loginname,\r\n  a.icon_files iconfiles,\r\n  a.district_id districtid,\r\n  a.create_time createtime,\r\n  b.login_count logincount,\r\n  b.last_login_time lastlogintime,\r\n  b.last_logout_time lastlogouttime,\r\n  b.last_active_time lastactivetime,\r\n  remark,\r\n (\r\n		select\r\n			group_concat(b.`name`)\r\n		from\r\n			sys_role b,\r\n			sys_user_role c\r\n		where\r\n			b.id = c.role_id\r\n		and c.user_id = a.id\r\n	) roleidlist\r\nfrom\r\n	sys_user a left JOIN sys_login b on a.login_name=b.login_name', '{\n    \"id\": {\n        \"title\": \"ID\",\n        \"type\": \"int\",\n        \"editable\": false,\n        \"filterAble\": false\n    },\n    \"name\": {\n        \"title\": \"姓名\",\n        \"type\": \"String\",\n        \"editable\": true\n    },\n    \"loginName\": {\n        \"title\": \"登录名\",\n        \"type\": \"String\",\n        \"editable\": true\n    },\n    \"districtId\": {\n        \"title\": \"归属地\",\n        \"type\": \"int\",\n        \"editable\": true\n    },\n    \"roleIdList\": {\n        \"title\": \"角色\",\n        \"type\": \"string\",\n        \"editable\": true,\n        \"editor\": {\n            \"type\": \"listAsyn\",\n            \"config\": {\n                \"api\": \"user/query/getListData\",\n                \"multiple\": true,\n                \"postEnt\": {\n                    \"code\": \"role\"\n                },\n                \"hasAllCheckBox\": false,\n                \"maxHeight\": 100\n            }\n        },\n        \"filter\": {\n            \"type\": \"listAsyn\",\n            \"config\": {\n                \"api\": \"user/query/getListData\",\n                \"postEnt\": {\n                    \"code\": \"role\"\n                }\n            }\n        }\n    },\n    \"iconFiles\": {\n        \"title\": \"头像图片\",\n        \"type\": \"int\",\n        \"editable\": true,\n        \"hide\": true,\n        \"filterable\": false,\n        \"editor\": {\n            \"type\": \"file\"\n        }\n    },\n\n    \"createTime\": {\n        \"title\": \"创建时间\",\n        \"type\": \"Date\",\n        \"editable\": false,\n        \"editor\": {\n            \"type\": \"Date\",\n            \"bsConfig\": {\n                \"dateInputFormat\": \"YYYY-MM-DD\",\n                \"containerClass\": \"theme-red\"\n            }\n        },\n        \"filter\": {\n            \"type\": \"Date\",\n            \"range\": true,\n            \"bsConfig\": {\n                \"dateInputFormat\": \"YYYY-MM-DD\",\n                \"containerClass\": \"theme-red\"\n            }\n        }\n    },\n    \"loginCount\": {\n        \"title\": \"登录次数\",\n        \"type\": \"int\",\n        \"editable\": false\n    },\n    \"lastLoginTime\": {\n        \"title\": \"最后登录时间\",\n        \"editable\": false,\n        \"type\": \"Date\",\n        \"editor\": {\n            \"type\": \"Date\",\n            \"bsConfig\": {\n                \"dateInputFormat\": \"YYYY-MM-DD\",\n                \"containerClass\": \"theme-red\"\n            }\n        }\n    },\n    \"lastLogoutTime\": {\n        \"title\": \"最后离开时间\",\n        \"type\": \"Date\",\n        \"editable\": false,\n        \"editor\": {\n            \"type\": \"Date\",\n            \"bsConfig\": {\n                \"dateInputFormat\": \"YYYY-MM-DD\",\n                \"containerClass\": \"theme-red\"\n            }\n        }\n    },\n    \"lastActiveTime\": {\n        \"title\": \"最后活动时间\",\n        \"type\": \"Date\",\n        \"editable\": false,\n        \"editor\": {\n            \"type\": \"Date\",\n            \"bsConfig\": {\n                \"dateInputFormat\": \"YYYY-MM-DD\",\n                \"containerClass\": \"theme-red\"\n            }\n        }\n    },\n    \"remark\": {\n        \"title\": \"备注\",\n        \"type\": \"String\",\n        \"editable\": true,\n        \"editor\": {\n            \"type\": \"textarea\"\n        }\n    },\n    \"isLocked\": {\n        \"title\": \"锁定\",\n        \"type\": \"String\",\n        \"editable\": true,\n        \"valuePrepareFunction\": (v) => { return v == 1 ? \'是\' : \'否\' },\n        \"editor\": {\n            \"type\": \"list\",\n            \"config\": {\n                \"list\": [\n                    {\n                        \"value\": \"1\",\n                        \"title\": \"是\"\n                    },\n                    {\n                        \"value\": \"0\",\n                        \"title\": \"否\"\n                    }\n                ]\n            }\n        }\n    }\n}', '', '', '[{\n		\"title\": \"修改\",\n		\"class\": \"nb-edit\",\n		\"apiUrl\": \"user/user/save\",\n		\"readUrl\": \"user/user/singleByKey\"\n	},\n	{\n		\"title\": \"删除\",\n		\"class\": \"nb-trash\",\n		\"apiUrl\": \"user/user/delete\",\n		\"confirmTip\": \"确定要删除该用户吗？\"\n	}\n]', '[\n    {\n        \"title\": \"添加\",\n        \"class\": \"nb-plus\",\n        \"click\": \"nowThis.Add(\'user/user/save\')\"\n    }\n]', '', '', '', '用户的基本情况管理');
INSERT INTO `sys_query` VALUES ('3', '模块管理', 'module', '1', '10', '1', '1', 'select \r\n  id,\r\n  parent_id parentid,\r\n  name,\r\n  location location,\r\n  code ,\r\n  is_debug isdebug,\r\n  is_hide ishide,\r\n  show_order showorder,\r\n  description description,\r\n  image_url imageurl,\r\n  desktop_role desktoprole,\r\n  w,\r\n  h \r\nfrom sys_module', '{\n    \"id\": {\n        \"title\": \"ID\",\n        \"type\": \"int\",\n        \"editable\": false\n    },\n    \"parentId\": {\n        \"title\": \"上级\",\n        \"type\": \"int\",\n        \"editable\": true,\n        \"editor\": {\n            \"type\": \"listTreeAsyn\",\n            \"config\": {\n                \"api\": \"user/query/getListData\",\n                \"postEnt\": {\n                    \"code\": \"module\",\n                    \"page\": 1,\n                    \"rows\": 20\n                },\n                \"TreeOptions\": {\n                    \"useCheckbox\": false,\n                    \"childrenField\": \"children\",\n                    \"displayField\": \"name\",\n                    \"idField\": \"id\"\n                },\n                \"hasAllCheckBox\": false,\n                \"maxHeight\": 100\n            }\n        }\n    },\n    \"name\": {\n        \"title\": \"模块名\",\n        \"type\": \"String\",\n        \"editable\": true\n    },\n    \"location\": {\n        \"title\": \"地址\",\n        \"type\": \"String\",\n        \"editable\": true\n    },\n    \"code\": {\n        \"title\": \"代码\",\n        \"type\": \"String\",\n        \"editable\": true\n    },\n    \"isDebug\": {\n        \"title\": \"调试\",\n        \"type\": \"checkbox\",\n        \"valuePrepareFunction\": (v) => { return v == 1 ? \'是\' : \'否\' },\n        \"editable\": true,\n        \"editor\": {\n            \"type\": \"checkbox\"\n        },\n        \"filter\": {\n            \"type\": \"checkbox\",\n            \"config\": {\n                \"true\": \"Yes\",\n                \"false\": \"No\",\n                \"resetText\": \"clear\"\n            }\n        }\n    },\n    \"isHide\": {\n        \"title\": \"隐藏\",\n        \"type\": \"String\",\n        \"editable\": true,\n        \"valuePrepareFunction\": (v) => { return v == 1 ? \'是\' : \'否\' },\n        \"editor\": {\n            \"type\": \"list\",\n            \"config\": {\n                \"list\": [\n                    {\n                        \"value\": \"1\",\n                        \"title\": \"是\"\n                    },\n                    {\n                        \"value\": \"0\",\n                        \"title\": \"否\"\n                    }\n                ]\n            }\n        }\n    },\n    \"showOrder\": {\n        \"title\": \"排序\",\n        \"type\": \"String\",\n        \"editable\": true\n    },\n    \"description\": {\n        \"title\": \"描述\",\n        \"type\": \"String\",\n        \"editable\": true\n    },\n    \"imageUrl\": {\n        \"title\": \"图片\",\n        \"type\": \"String\",\n        \"editable\": true\n    },\n    \"desktopRole\": {\n        \"title\": \"桌面角色\",\n        \"type\": \"String\",\n        \"editable\": true\n    },\n    \"w\": {\n        \"title\": \"宽\",\n        \"type\": \"int\",\n        \"editable\": true\n    },\n    \"h\": {\n        \"title\": \"高\",\n        \"type\": \"int\",\n        \"editable\": true\n    }\n}', '', '', '[\n    {\n        \"title\": \"修改\",\n        \"class\": \"nb-edit\",\n        \"readUrl\": \"user/module/singleByKey\",\n        \"apiUrl\": \"user/module/save\"\n    },\n    {\n        \"title\": \"删除\",\n        \"class\": \"nb-trash\",\n        \"apiUrl\": \"user/module/delete\",\n        \"confirmTip\": \"确定要删除吗？\"\n    }\n]', '[{\n		\"title\": \"添加\",\n		\"class\": \"nb-plus\",\n		\"click\": \"nowThis.Add(\'user/module/save\')\"\n	},\n	{\n		\"title\": \"批量删除\",\n		\"class\": \"ion-delete\",\n		\"click\": \"nowThis.Exec(\'user/module/delete\',\'ID\',\'删除要删除吗？\')\"\n	}\n]', '', '', '', '模块管理');
INSERT INTO `sys_query` VALUES ('4', '角色管理', 'role', '1', '10', '1', '1', 'select id , name,remark from sys_role', '\r\n{\r\n  \"id\": {\r\n	\"title\": \"角色ID\",\r\n	\"defaultValue\": 0,\r\n	\"type\": \"number\",\r\n	\"editable\": false\r\n  },\r\n  \"name\": {\r\n	\"title\": \"角色名\",\r\n	\"type\": \"string\"\r\n  },\r\n  \"remark\": {\r\n	\"title\": \"备注\",\r\n	\"type\": \"string\"\r\n  },\r\n  \"moduleIdStr\": {\r\n	\"title\": \"所有模块\",\r\n	\"inputWidth\":\"12\",\r\n	\"type\": \"treeview\",\r\n	\"editor\":{\"type\": \"treeview\"}\r\n  }\r\n}\r\n', '', '', '[{\n		\"title\": \"修改\",\n		\"class\": \"nb-edit\",\n		\"openModal\": \"RoleEditComponent\",\n		\"readUrl\": \"user/role/singleByKey\",\n		\"apiUrl\": \"user/role/save\"\n	},\n	{\n		\"title\": \"删除\",\n		\"class\": \"nb-trash\",\n		\"apiUrl\": \"user/role/delete\",\n		\"confirmTip\": \"确定要删除吗？\"\n	}\n]', '[{\n		\"title\": \"添加\",\n		\"class\": \"nb-plus\",\n		\"click\": \"nowThis.Add(\'user/role/save\',\'RoleEditComponent\',{Key:0},\'user/role/singleByKey\')\"\n	},\n	{\n		\"title\": \"批量删除\",\n		\"class\": \"ion-delete\",\n		\"click\": \"nowThis.Exec(\'user/role/delete\',\'ID\',\'删除要删除吗？\')\"\n	},\n	{\n		\"title\": \"导出\",\n		\"class\": \"nb-archive\",\n		\"click\": \"nowThis.onExportXls()\"\n	}\n]', '', '', '', '管理角色');

-- ----------------------------
-- Table structure for `sys_role`
-- ----------------------------
DROP TABLE IF EXISTS `sys_role`;
CREATE TABLE `sys_role` (
  `id` int(11) NOT NULL,
  `name` varchar(80) NOT NULL DEFAULT '' COMMENT '角色名称',
  `remark` varchar(255) NOT NULL DEFAULT '' COMMENT '备注',
  `type` int(11) NOT NULL DEFAULT 0 COMMENT '类型',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sys_role
-- ----------------------------
INSERT INTO `sys_role` VALUES ('1', '系统管理员', '', '0');

-- ----------------------------
-- Table structure for `sys_role_module`
-- ----------------------------
DROP TABLE IF EXISTS `sys_role_module`;
CREATE TABLE `sys_role_module` (
  `role_id` int(11) NOT NULL,
  `module_id` int(11) NOT NULL,
  PRIMARY KEY (`role_id`,`module_id`),
  KEY `FK_sys_role_module_REF_module` (`module_id`),
  CONSTRAINT `FK_sys_role_module_REF_module` FOREIGN KEY (`module_id`) REFERENCES `sys_module` (`id`),
  CONSTRAINT `FK_sys_role_module_REF_role` FOREIGN KEY (`role_id`) REFERENCES `sys_role` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sys_role_module
-- ----------------------------
INSERT INTO `sys_role_module` VALUES ('1', '1');
INSERT INTO `sys_role_module` VALUES ('1', '2');
INSERT INTO `sys_role_module` VALUES ('1', '3');
INSERT INTO `sys_role_module` VALUES ('1', '4');
INSERT INTO `sys_role_module` VALUES ('1', '5');

-- ----------------------------
-- Table structure for `sys_sequence`
-- ----------------------------
DROP TABLE IF EXISTS `sys_sequence`;
CREATE TABLE `sys_sequence` (
  `seq_name` varchar(50) NOT NULL COMMENT '表名',
  `current_val` int(11) NOT NULL DEFAULT 0 COMMENT '当前ID',
  `increment_val` int(11) NOT NULL DEFAULT 1 COMMENT '每次增加值',
  PRIMARY KEY (`seq_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sys_sequence
-- ----------------------------
INSERT INTO `sys_sequence` VALUES ('sys_files', '7', '1');
INSERT INTO `sys_sequence` VALUES ('wx_quest_log', '45', '1');

-- ----------------------------
-- Table structure for `sys_user`
-- ----------------------------
DROP TABLE IF EXISTS `sys_user`;
CREATE TABLE `sys_user` (
  `id` int(11) NOT NULL,
  `name` varchar(80) NOT NULL COMMENT '姓名',
  `login_name` varchar(20) NOT NULL COMMENT '登录名',
  `icon_files` varchar(100) NOT NULL DEFAULT '' COMMENT '文件hashcode',
  `district_id` int(11) NOT NULL COMMENT '类型',
  `create_time` bigint(20) NOT NULL DEFAULT 0,
  `status` smallint(6) NOT NULL DEFAULT 0 COMMENT '状态',
  `remark` varchar(100) NOT NULL DEFAULT '' COMMENT '备注',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sys_user
-- ----------------------------
INSERT INTO `sys_user` VALUES ('1', 'admin', '18180770313', '202008\\0.png', '0', '0', '0', '111');

-- ----------------------------
-- Table structure for `sys_user_role`
-- ----------------------------
DROP TABLE IF EXISTS `sys_user_role`;
CREATE TABLE `sys_user_role` (
  `role_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  PRIMARY KEY (`role_id`,`user_id`),
  KEY `FK_FA_USER_ROLE_REF_USER` (`user_id`),
  CONSTRAINT `FK_FA_USER_ROLE_REF_ROLE` FOREIGN KEY (`role_id`) REFERENCES `sys_role` (`id`),
  CONSTRAINT `FK_FA_USER_ROLE_REF_USER` FOREIGN KEY (`user_id`) REFERENCES `sys_user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sys_user_role
-- ----------------------------
INSERT INTO `sys_user_role` VALUES ('1', '1');

-- ----------------------------
-- Table structure for `wx_quest_log`
-- ----------------------------
DROP TABLE IF EXISTS `wx_quest_log`;
CREATE TABLE `wx_quest_log` (
  `id` int(11) NOT NULL,
  `to_user_name` varchar(100) NOT NULL DEFAULT '',
  `from_user_name` varchar(100) NOT NULL DEFAULT '',
  `msg_type` varchar(100) NOT NULL DEFAULT '',
  `content` varchar(1000) NOT NULL DEFAULT '',
  `event_type` varchar(100) NOT NULL DEFAULT '',
  `event_key` varchar(100) NOT NULL DEFAULT '',
  `ticket` varchar(100) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of wx_quest_log
-- ----------------------------
INSERT INTO `wx_quest_log` VALUES ('1', '', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'subscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('2', '', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'unsubscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('6', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'unsubscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('7', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'subscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('9', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'unsubscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('11', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'subscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('13', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'subscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('15', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'subscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('17', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'subscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('19', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'subscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('21', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'subscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('23', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'subscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('25', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'subscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('27', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'subscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('29', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'subscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('31', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'unsubscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('33', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'unsubscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('35', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'unsubscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('37', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'unsubscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('39', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'unsubscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('41', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'unsubscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('43', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'unsubscribe', '', '');
INSERT INTO `wx_quest_log` VALUES ('45', 'gh_79f9a3893207', 'oJwhPxIiFt7npgUpPF4tOhGS3YMI', 'event', '', 'unsubscribe', '', '');

-- ----------------------------
-- Table structure for `wx_user`
-- ----------------------------
DROP TABLE IF EXISTS `wx_user`;
CREATE TABLE `wx_user` (
  `openid` varchar(50) NOT NULL DEFAULT '' COMMENT '用户唯一标识',
  `nickname` varchar(100) NOT NULL DEFAULT '' COMMENT '用户昵称',
  `sex` varchar(100) NOT NULL DEFAULT '' COMMENT '用户的性别，值为1时是男性，值为2时是女性，值为0时是未知',
  `province` varchar(1000) NOT NULL DEFAULT '' COMMENT '用户个人资料填写的省份',
  `city` varchar(100) NOT NULL DEFAULT '' COMMENT '普通用户个人资料填写的城市',
  `country` varchar(100) NOT NULL DEFAULT '' COMMENT '国家，如中国为CN',
  `headimgurl` varchar(500) NOT NULL DEFAULT '' COMMENT '用户头像',
  `unionid` varchar(100) NOT NULL DEFAULT '' COMMENT '只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段',
  `ip` varchar(16) NOT NULL DEFAULT '' COMMENT 'ip地址',
  `address` varchar(20) NOT NULL DEFAULT '' COMMENT '地址',
  `create_time` bigint(20) NOT NULL COMMENT '创建时间',
  `last_time` bigint(20) NOT NULL COMMENT '最后时间',
  `language` varchar(20) NOT NULL DEFAULT '' COMMENT '用户的语言，简体中文为zh_CN',
  `subscribe` int(11) NOT NULL COMMENT '用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。',
  `subscribe_time` bigint(20) NOT NULL COMMENT '用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间',
  `remark` varchar(100) NOT NULL DEFAULT '' COMMENT '公众号运营者对粉丝的备注',
  `groupid` int(11) NOT NULL COMMENT '用户所在的分组ID',
  `tagid_list_str` varchar(100) NOT NULL DEFAULT '' COMMENT '用户被打上的标签ID列表',
  `subscribe_scene` varchar(30) NOT NULL DEFAULT '' COMMENT '户关注的渠道来源',
  `qr_scene` varchar(10) NOT NULL DEFAULT '' COMMENT '二维码扫码场景',
  `qr_scene_str` varchar(10) NOT NULL DEFAULT '' COMMENT '二维码扫码场景描述',
  PRIMARY KEY (`openid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of wx_user
-- ----------------------------
INSERT INTO `wx_user` VALUES ('oJwhPxIiFt7npgUpPF4tOhGS3YMI', '翁志来', '1', '四川', '南充', '中国', 'http://thirdwx.qlogo.cn/mmopen/q8cn3WLCTWnM1b4jQWEay5DSTHP3aRnic3kLcU94JK6fEGekBKygL33jb0AAa8uicHdlmnU4BbyFicGNWMT8uelJDkSmquKMAmN/132', '', '127.0.0.1', '未知未知', '20200806142740', '20200806151435', 'zh_CN', '0', '1596700101', '', '0', '', 'ADD_SCENE_QR_CODE', '0', '');
