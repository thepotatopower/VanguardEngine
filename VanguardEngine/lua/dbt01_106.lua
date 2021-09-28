--Downswing of Sword of Judgement

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Grade, 3, q.Count, 1
	elseif n == 2 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Selected, q.Count, 1
	elseif n == 4 then
		return q.Location, l.EnemyRC, q.Column, 0
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, p.HasPrompt, p.SpecificSB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(2) then
			return true
		end
	end
	return false
end

function Activate(n)
	local column = 0
	if n == 1 then
		obj.Select(2)
		if obj.Exists(3) then
			column = obj.GetColumn(3)
			obj.Inject(4, q.Column, column)
			obj.SendToBottom(4)
		end
	end
	return 0
end