-- Electro Spartan

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerHand, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Damage, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	elseif n == 2 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRodeUponThisTurn() and obj.VanguardIs("Master of Gravity, Baromagnes") and obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		if obj.LastPlacedOnRC() and obj.CanCB(2) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.ChooseAddToSoul(1)
	elseif n == 2 then
		obj.CounterBlast(2)
	end
end

function Activate(n)
	if n == 1 then
		obj.Draw(1)
		obj.SoulCharge(1)
		obj.OnRideAbilityResolved()
	elseif n == 2 then
		obj.SoulCharge(2)
	end
	return 0
end