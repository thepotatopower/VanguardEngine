-- Knight of Friendship, Cryrus

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Revealed, q.Other, o.Unit, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Revealed
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, t.ACT, p.HasPrompt, p.OncePerTurn, p.SB, 2
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() then
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
		if obj.Exists(1) then
			obj.SuperiorCall(1)
		else
			obj.AddToHand(2)
		end
	end
	return 0
end