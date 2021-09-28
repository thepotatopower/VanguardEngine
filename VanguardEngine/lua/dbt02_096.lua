-- Octadevote Sorceress

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
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnDriveCheck, p.HasPrompt, p.Retire, 2
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsPlayerTurn() and obj.Exists(1) and obj.VanguardIs("Hexaorb Sorceress") then
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

function Cost(n)
	if n == 1 then
		obj.Retire(2)
	end
end

function Activate(n)
	if n == 1 then
		if obj.SelectOption("Counter Charge 1", "Soul Charge 1") == 1 then
			obj.CounterCharge(1)
		else
			obj.SoulCharge(1)
		end
	end
	return 0
end