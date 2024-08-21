from flask import Flask, request, jsonify
import tempfile
import os
import subprocess
import re
import json

app = Flask(__name__)

@app.route('/run', methods=['Post'])
def run_command():
    # 获取请求参数
    print("什么东西")
    x = request.form.get('x')
    y = request.form.get('y')
    print(x)
    print(y)
    if x is None or y is None:
        return jsonify({"error": "Missing parameters x or y"}), 400
    print("dddd")
    # 创建临时目录
    temp_dir = tempfile.mkdtemp()
    # 文件路径
    print(temp_dir)
    domain_file_path = os.path.join(temp_dir, 'Domain.pddl')
    problem_file_path = os.path.join(temp_dir, 'Problem.pddl')
    result_file_path = os.path.join(temp_dir, 'Problem.pddl.plan')
    # 保存参数到文件
    with open(domain_file_path, 'w') as domain_file:
        domain_file.write(x)
    
    with open(problem_file_path, 'w') as problem_file:
        problem_file.write(y)
    print(domain_file_path)
    print(problem_file_path)
    # 执行 shell 脚本并传递临时目录路径作为参数
    result = subprocess.run(
        ["/bin/bash", "test.sh", domain_file_path,problem_file_path],
        capture_output=True,
        text=True
    )
    # with open(result_file_path, 'w') as result_file:
    #     result_file.read(y)
    print(result)
    
    # with open(result_file_path, 'r') as result_file:
    #     output_file = result_file.read()

    # 返回脚本的输出作为 HTTP 响应
    return parse_pddl_output(result_file_path)

def parse_pddl_output(file_path):
    parsed_data = []

    # Regular expression to match the time and action part
    pattern = re.compile(r'(\d+\.\d+):\s*\(([^)]+)\)\s*\[.*\]')

    with open(file_path, 'r') as file:
        print(file_path)
        for line in file:
            print("ha?")
            print(line)
            match = pattern.match(line.strip())
            if match:
                action = match.group(2).strip()
                parameters = action.split()
                parsed_data.append(parameters)
    print("result:")
    print(parsed_data)
    ss= json.dumps(parsed_data, indent=4)
    print(ss)
    return ss

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=80, debug=True)