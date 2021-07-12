-- Knight of Heavenly Sword, Fort

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerHand, q.Grade, 3, q.Count, 2
	elseif n == 2 then
		return q.Location, l.Revealed, q.Other, o.Unit, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 4 then
		return q.Location, l.PlayerVC, q.Count, 1
	elseif n == 5 then
		return q.Location, l.Revealed
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	elseif n == 2 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRodeUponThisTurn() and obj.VanguardIs("Knight of Heavenly Spear, Rooks") and obj.CanReveal(1) then
			return true
		end
	elseif n == 2 then
		if obj.NotActivatedYet() and obj.IsRearguard() and obj.CanAddPower(4) and obj.CanCB(3) then
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
	if n == 1 then
		obj.ChooseReveal(1)
	elseif n == 2 then
		obj.CounterBlast(3)
	end
end

function Activate(n)
	if n == 1 then
		obj.RevealFromDeck(1)
		if obj.CanSuperiorCall(2) then
			obj.SuperiorCall(2)
		else
			obj.AddToDrop(5)
		end
		obj.EndReveal()
		obj.OnRideAbilityResolved()
	elseif n == 2 then
		obj.ChooseAddTempPower(4, 5000)
	end
	return 0
end