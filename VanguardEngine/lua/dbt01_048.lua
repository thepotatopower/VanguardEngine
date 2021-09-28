-- Divine Sister, Faciata

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.RevealedTrigger, q.UnitType, u.Trigger, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Damage, q.Other, o.FaceDown, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnDriveCheck, p.HasPrompt, p.OncePerTurn, p.SB, 2
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsPlayerTurn() and obj.Exists(1) then
			return true
		end
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
	if n == 1 then
		obj.CounterCharge(1)
	end
	return 0
end