-- Dragon Knight, Nehalem

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.NameContains, "Overlord", q.Count, 1
	elseif n == 2 then
		return q.Location, l.Soul, q.Other, o.This, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 4 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 5 then
		return q.Location, l.PlayerVC
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, t.Auto, p.HasPrompt, true, p.IsMandatory, true
	elseif n == 2 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.Exists(2) and obj.IsRodeUponThisTurn() and obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and not obj.Activated() and obj.Exists(1) and obj.CanSB(3) then
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

function Cost(n)
	if n == 2 then
		obj.SoulBlast(3)
	end
end

function Activate(n)
	if n == 1 then
		obj.SuperiorCall(2)
		obj.OnRideAbilityResolved()
	elseif n == 2 then
		obj.AddTempPower(4, 5000)
		obj.AddTempPower(5, 5000)
	end
	return 0
end