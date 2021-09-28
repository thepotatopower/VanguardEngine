-- Dragon Knight, Nehalem

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.NameContains, "Overlord", q.Count, 1
	elseif n == 2 then
		return q.Location, l.Soul, q.Other, o.This, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 4 then
		return q.Location, l.PlayerVC
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, p.HasPrompt, p.IsMandatory
	elseif n == 2 then
		return a.OnACT, p.HasPrompt, p.OncePerTurn, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.WasRodeUponByNameContains("Overlord") and obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.Exists(1) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.SuperiorCall(2)
	elseif n == 2 then
		obj.AddTempPower(3, 5000)
		obj.AddTempPower(4, 5000)
	end
	return 0
end