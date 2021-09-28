-- Knight of Heavenly Collapse, Capaldo

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.Standing, q.Other, o.NotThis, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Revealed, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, p.HasPrompt, p.CB, 1, p.Rest, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() then
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
		obj.RevealFromDeck(1)
		if obj.GetSelectedGrade(2) == 3 or obj.GetSelectedUnitType(2) == -1 then
			obj.AddToHand(2)
		else
			obj.SuperiorCall(2)
		end
	end
	return 0
end