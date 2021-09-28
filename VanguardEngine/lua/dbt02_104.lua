-- Blaster Javelin

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Revealed, q.Other, o.Unit
	elseif n == 2 then
		return q.Location, l.Revealed
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 4 then
		return q.Location, l.PlayerVC, q.NameContains, "Blaster", q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnVC, p.HasPrompt, p.IsMandatory
	elseif n == 2 then
		return a.Cont, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.WasRodeUponBy("Blaster Dark") then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() then
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
		obj.RevealFromDeck(1)
		if obj.Exists(1) then
			obj.SuperiorCallAsRest(1)
		else
			obj.AddToDrop(2)
		end
	elseif n == 2 then
		if obj.IsPlayerTurn() and obj.Exists(4) then
			obj.SetAbilityPower(3, 2000)
		end
	end
	return 0
end
