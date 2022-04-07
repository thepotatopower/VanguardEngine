-- Black Tears Husk Dragon

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then 
		return q.Location, l.Drop, q.Other, o.NormalOrder, q.Count, 1, q.Min, 0
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnVC, p.HasPrompt
	elseif n == 2 then
		return a.Cont, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnVC() and obj.Exists(1) then
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
		return false
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.ChooseAddToHand(1)
	elseif n == 2 then
		if obj.OrderPlayed() then
			obj.SetAbilityPower(2, 5000)
		end
	end
	return 0
end