-- Armored Dragon, Mountcannon

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.FrontRow, q.Location, l.PlayerRC, q.Other, o.This, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Grade, 0, q.Other, o.Attacking, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.Exists(1) and obj.Exists(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.AddTempPower(1, 5000)
		obj.AddTempPower(2, 5000)
	end
	return 0
end