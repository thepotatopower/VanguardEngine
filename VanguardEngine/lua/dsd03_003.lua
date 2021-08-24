-- Knight of Heavenly Sword, Fort

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerHand, q.Grade, 3, q.Count, 2
	elseif n == 2 then
		return q.Location, l.Revealed, q.Other, o.Unit, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerVC, q.Count, 1
	elseif n == 4 then
		return q.Location, l.Revealed
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, t.Auto, p.HasPrompt, p.Reveal, 1
	elseif n == 2 then
		return a.OnACT, t.ACT, p.HasPrompt, p.OncePerTurn, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.WasRodeUponBy("Knight of Heavenly Spear, Rooks") then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.Exists(4) then
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
		if obj.Exists(2) then
			obj.SuperiorCall(2)
		end
		obj.AddToDrop(4)
		obj.EndReveal()
	elseif n == 2 then
		obj.ChooseAddTempPower(3, 5000)
	end
	return 0
end