-- Knight of Friendship, Cryrus

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Count, 2
	elseif n == 2 then
		return q.Location, l.Revealed, q.Other, o.Unit, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Revealed
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.NotActivatedYet() and obj.IsRearguard() and obj.CanSB(1) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.SoulBlast(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.RevealFromDeck(1)
		if obj.Exists(2) then
			obj.SuperiorCall(2)
		else
			obj.AutoAddToHand(3)
		end
	end
	return 0
end