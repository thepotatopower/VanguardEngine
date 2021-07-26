-- Coffin Shooter

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Drop, q.Other, o.Order, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Drop, q.Other, o.This, q.Count, 1
	elseif n == 4 then
		return q.Location, l.PlayerRC
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.Exists(3) and not obj.Activated() and obj.CanSB(1) and obj.Exists(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.OpenCirclesExist(4) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.ChooseBind(2)
		obj.SoulBlast(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.SuperiorCall(3, FL.OpenCircle)
	end
	return 0
end