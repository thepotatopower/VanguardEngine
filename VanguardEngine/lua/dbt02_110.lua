-- Coffin Shooter

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Drop, q.Other, o.Order, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Drop, q.Other, o.This, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, p.HasPrompt, p.OncePerTurn, p.SB, 1, p.Bind, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.Exists(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.OpenCirclesExist(3) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.SuperiorCallToSpecificCircle(2, FL.OpenCircle)
	end
	return 0
end