-- Deep Soniker

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnVC, t.Auto, p.HasPrompt, true, p.IsMandatory, true
	elseif n == 2 then
		return a.Cont, t.Cont, p.HasPrompt, false, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnVC() then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() then
			return true
		end
	end
	return false
end

function Cost(n)
end

function Activate(n)
	if n == 1 then
		obj.SoulCharge(1)
	elseif n == 2 then
		if obj.IsPlayerTurn() and obj.SoulCount() >= 10 then
			obj.SetAbilityPower(2, 10000)
		else
			obj.SetAbilityPower(2, 0)
		end
	end
	return 0
end