-- Heavy Artillery of Dust Storm, Eugene

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Count, 2, q.Other, o.Standing
	elseif n == 2 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerVC, q.Other, o.This
	elseif n == 4 then
		return q.Location, l.Soul, q.Count, 5
	elseif n == 5 then
		return q.Location, l.Looking
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false, p.Description, "Rest two rear-guards, choose one of your opponent's rear-guards, retire it, and this unit gets Power+10000 until end of turn."
	elseif n == 2 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false, p.Description, "Soul Blast 5, look at the same number of cards from the top of your deck as the number of your opponent's open RC, choose any number of unit cards from among them, call them to RC, and put the rest into your soul."
	end
end

function CheckCondition(n)
	if n == 1 then
		if not obj.Activated() and obj.IsVanguard() and obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		if not obj.Activated() and obj.IsVanguard() and obj.EnemyRetiredThisTurn() and obj.CanSB(4) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(2) then
			return true
		end
	elseif n == 2 then
		return true
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.ChooseRest(1)
	elseif n == 2 then
		obj.SoulBlast(4)
	end
end

function Activate(n)
	if n == 1 then
		if obj.Exists(2) then
			obj.ChooseRetire(2)
		end
		obj.AddTempPower(3, 10000)
	elseif n == 2 then
		obj.LookAtTopOfDeck(obj.NumEnemyOpenCircles())
		obj.SuperiorCall(5)
		obj.AddToSoul(5)
	end
	return 0
end