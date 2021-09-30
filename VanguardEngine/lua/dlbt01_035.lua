-- Talent of Enjoyment, Feltyrosa

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerHand, q.Race, r.Ghost, q.UnitType, u.Normal, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Drop, q.Race, r.Ghost, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Race, r.Ghost, q.Count, 3
	elseif n == 4 then
		return q.Location, l.PlayerVC, q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, p.HasPrompt, p.SendToTop, 1
	elseif n == 2 then
		return a.Cont, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsApplicable() then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() or obj.IsRearguard() then
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
		obj.ChooseAddToHand(2)
	elseif n == 2 then
		if obj.IsPlayerTurn() and obj.Exists(3) then
			obj.SetAbilityPower(4, 5000)
		end
	end
	return 0
end