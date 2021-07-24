-- Knight of Heavenly Collapse, Capaldo

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.Standing, q.Other, o.NotThis, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Revealed, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() and obj.CanCB(1) and obj.Exists(2) then
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
		obj.CounterBlast(1)
		obj.ChooseRest(2)
	end
end

function Activate(n)
	if n == 1 then
		obj.RevealFromDeck(1)
		if obj.GetSelectedGrade(3) == 3 or obj.GetSelectedUnitType(3) == -1 then
			obj.AddToHand(3)
		else
			obj.SuperiorCall(3)
		end
	end
	return 0
end